using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.AI;

public class Unit : NetworkBehaviour
{
    public UnitSO unitSO;

    private NetworkAnimator animator;
    private NavMeshAgent agent;
    private ulong ID;
    private Vector3 target;

    private int currentAttack;
    private bool isAttacking;

    private const float StopThreshold = 0.01f;
    private bool IsStopped => agent.velocity.sqrMagnitude < StopThreshold;

    private void Awake()
    {
        animator = GetComponent<NetworkAnimator>();
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
            unitSO.movement.Move(target);

            if (IsStopped && !isAttacking && transform.position.IsDistanceFromTargetInRange(target, unitSO.attacks[currentAttack].range))
            {
                animator.Animator.SetFloat("speed", 0);
                Debug.Log(currentAttack);
                animator.Animator.SetFloat("attack", currentAttack);
                animator.Animator.SetTrigger("IsAttacking");
                AttackingClientRpc();
                isAttacking = true;
                unitSO.attacks[currentAttack].Attack();
            }

            if (isAttacking && unitSO.attacks[currentAttack].IsCooldownOver)
            {
                currentAttack++;
                currentAttack %= unitSO.attacks.Count;
                Debug.Log(unitSO.attacks.Count + " " + currentAttack);
                isAttacking = false;
            }
        }
    }

    [ClientRpc]
    private void AttackingClientRpc()
    {
        animator.Animator.SetTrigger("IsAttacking");
    }
}

