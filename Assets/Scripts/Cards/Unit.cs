using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : NetworkBehaviour, IHitable
{
    public UnitSO unitSO;
    
    private ulong ID;
    
    private UnitState state = UnitState.Idle;
    private int currentAttack;
    
    private NavMeshAgent agent;
    private Graphics graphics;
    private IHitable target;
    public IHitable.Death OnDeath { get; set; }
    
    [SerializeField]
    private NetworkVariable<Stats> networkStats = new NetworkVariable<Stats>();
    
    [SerializeField]
    private NetworkVariable<Stats> initialStats = new NetworkVariable<Stats>();
    
    private bool IsTargetReached => transform.position.IsDistanceFromTargetInRange(target.transform.position, agent.stoppingDistance);
    private bool IsTargetInAttackRange => transform.position.IsDistanceFromTargetInRange(target.transform.position, unitSO.attacks[currentAttack].range);

    public Image UIHealth;

    private void Awake()
    {
        graphics = GetComponent<Graphics>();
        graphics.Init(GetComponent<NetworkAnimator>());
    }

    public void ServerInit()
    {
        if (IsOwner)
        {
            unitSO = GameObject.Instantiate(unitSO);
            unitSO.Init();
            agent = GetComponent<NavMeshAgent>();
            unitSO.movement.Init(agent);
            OnDeath += DestroySelf;
        }
    }

    public void IdInit(ulong id)
    {
        ID = id;
        unitSO.priority.Init(id);
    }

    private void Update()
    {
        if (IsOwner)
        {
            OnAnyState();
            
            switch (state)
            {
                case UnitState.Idle:
                    OnIdle();
                    break;
                case UnitState.Walking:
                    OnMoving();
                    break;
                case UnitState.Attacking:
                    OnAttacking();
                    break;
            }

            if (networkStats.Value != unitSO.stats.value)
                networkStats.Value = unitSO.stats.value;
            
            if (initialStats.Value != unitSO.stats.InitialValue)
                initialStats.Value = unitSO.stats.InitialValue;
        }
        
        UIHealth.fillAmount = networkStats.Value.Hp / initialStats.Value.Hp;
    }

    private void OnAnyState()
    {
        target = unitSO.priority.GetTarget(transform.position);
        
        //Rotate toward target
        Vector3 targetDirection = target.transform.position - transform.position;

        transform.rotation =
            Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, 1.0f, 0.0f));
    }

    private void OnMovingEnter()
    {
        state = UnitState.Walking;
        graphics.MoveAnimation(true);
    }

    private void OnMoving()
    {
        unitSO.movement.Move(target.transform.position);

        if (IsTargetReached)
        {
            OnMovingExit();
            
            if (IsTargetInAttackRange)
                OnAttackingEnter();
            else
                OnIdleEnter();
        }
    }

    private void OnMovingExit()
    {
        graphics.MoveAnimation(false);
    }

    private void OnAttackingEnter()
    {
        state = UnitState.Attacking;
        Attack();
    }

    private void OnAttacking()
    {
        if (unitSO.attacks[currentAttack].IsCooldownOver)
        {
            ChangeAttack();
            Attack();
        }
        
        if (!IsTargetReached)
        {
            OnAttackingExit();
            OnMovingEnter();
        }
    }

    private void OnAttackingExit()
    {
        currentAttack = 0;
    }

    private void OnIdleEnter()
    {
        state = UnitState.Idle;
    }

    private void OnIdle()
    {
        if (!IsTargetReached)
            OnMovingEnter();
        else if (IsTargetInAttackRange)
            OnAttackingEnter();
    }

    private void Attack()
    {
        graphics.AttackAnimation(currentAttack);
        unitSO.attacks[currentAttack].Attack(target);
    }
    
    private void ChangeAttack()
    {
        currentAttack++;
        currentAttack %= unitSO.attacks.Count;
    }
    
    public void ResetSelf()
    {
        currentAttack = 0;
        state = UnitState.Idle;
        graphics.ResetSelf();
        unitSO.stats.ResetSelf();
    }
    
    public void OnHit(Stats stats)
    {
        unitSO.stats.Hit(stats);
        
        if (unitSO.stats.IsDead)
            OnDeath.Invoke();
    }

    private void DestroySelf()
    {
        GameManager.Instance.GetUnits(ID).Remove(this);
        Pool.Instance.PoolUnit(this);
    }
}