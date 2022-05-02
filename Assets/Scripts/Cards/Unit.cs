using Unity.Netcode;
using UnityEngine.AI;

public class Unit : NetworkBehaviour
{
    public UnitSO unitSO;
    private NavMeshAgent agent;
    public ulong owner;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        unitSO.movement.Init(agent);
    }

    public void Init(ulong id)
    {
        owner = id;
    }

    private void Update()
    {
    }
}
