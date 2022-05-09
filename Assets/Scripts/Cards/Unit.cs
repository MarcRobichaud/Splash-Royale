using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : NetworkBehaviour, IHitable
{
    public UnitSO unitSO;
    
    private NavMeshAgent agent;
    private Graphics graphics;
    private ulong ID;
    private IHitable target;
    
    public IHitable.Death OnDeath { get; set; }
    
    [SerializeField]
    private NetworkVariable<Stats> networkStats = new NetworkVariable<Stats>();
    
    [SerializeField]
    private NetworkVariable<Stats> initialStats = new NetworkVariable<Stats>();

    private int currentAttack;
    private bool isAttacking;

    private const float StopThreshold = 0.01f;
    private bool IsStopped => agent.velocity.sqrMagnitude < StopThreshold;
    private bool isMoving;

    public Image UIHealth;

    private void Awake()
    {
        graphics = GetComponent<Graphics>();
        graphics.Init(GetComponent<NetworkAnimator>());
        OnDeath += DestroySelf;
    }

    public void ServerInit(ulong id)
    {
        if (IsOwner)
        {
            unitSO = GameObject.Instantiate(unitSO);
            unitSO.Init();
            agent = GetComponent<NavMeshAgent>();
            ID = id;
            unitSO.movement.Init(agent);
            unitSO.priority.Init(id);
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            target = unitSO.priority.GetTarget(transform.position);
            unitSO.movement.Move(target.transform.position);
            
            Vector3 targetDirection = target.transform.position - transform.position;
            
            transform.rotation =
                Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, 1.0f, 0.0f));
            
            if (isAttacking && unitSO.attacks[currentAttack].IsCooldownOver)
                ChangeAttack();

            if (IsStopped && !isAttacking && transform.position.IsDistanceFromTargetInRange(target.transform.position, unitSO.attacks[currentAttack].range))
            {
                StartAttack();
            }

            if (!IsStopped && !isMoving)
            {
                graphics.MoveAnimation(true);
                isMoving = true;
            }
            
            if (IsStopped && !isAttacking && isMoving)
            {
                graphics.MoveAnimation(false);
                isMoving = false;
            }

            if (networkStats.Value != unitSO.stats.value)
                networkStats.Value = unitSO.stats.value;
            
            if (initialStats.Value != unitSO.stats.InitialValue)
                initialStats.Value = unitSO.stats.InitialValue;
        }
        
        UIHealth.fillAmount = networkStats.Value.Hp / initialStats.Value.Hp;
    }

    private void ChangeAttack()
    {
        currentAttack++;
        currentAttack %= unitSO.attacks.Count;
        isAttacking = false;
    }

    private void StartAttack()
    {
        graphics.AttackAnimation(currentAttack);
        isAttacking = true;
        unitSO.attacks[currentAttack].Attack(target);
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
        Destroy(gameObject);
    }
}

