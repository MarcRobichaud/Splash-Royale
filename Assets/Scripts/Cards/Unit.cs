using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class Unit : NetworkBehaviour
{
    public UnitSO unitSO;

    private Animator animator;
    private NavMeshAgent agent;
    private ulong owner;
    private Vector3 target;

    public void Init(ulong id)
    {
        if (IsOwner)
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            owner = id;
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
        }
    }
}
