using UnityEngine;
using UnityEngine.AI;

public abstract class MovementSO : ScriptableObject
{
    public float MovementSpeed;

    public abstract void Init(float range, NavMeshAgent agent = null);
    public abstract void Move(Vector3 destination);
}
