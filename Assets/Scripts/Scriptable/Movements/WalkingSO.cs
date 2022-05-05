using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Walking", menuName = "ScriptableObjects/Movement/Walking SO")]
public class WalkingSO : MovementSO
{
    private NavMeshAgent agent;
    
    public override void Init(NavMeshAgent agent = null)
    {
        this.agent = agent;
        
        if (agent)
            agent.speed = MovementSpeed;
    }

    public override void Move(Vector3 destination)
    {
        if (agent != null && agent.destination != destination)
            agent.destination = destination;
    }
}
