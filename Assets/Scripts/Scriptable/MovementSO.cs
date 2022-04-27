using UnityEngine;

public abstract class MovementSO : ScriptableObject
{
    public float MovementSpeed;
    public abstract void Move();
}
