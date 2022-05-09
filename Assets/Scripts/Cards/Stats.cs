using System;
using Unity.Netcode;

[Serializable]
public struct Stats : INetworkSerializeByMemcpy
{
    public float Hp;
    public float MovementSpeed;
    public float AttackSpeed;
    public float Damage;

    private const float TOLERANCE = 0.01f;

    public Stats(float hp, float movementSpeed = 1f, float attackSpeed = 1f, float damage = 1f)
    {
        Hp = hp;
        MovementSpeed = movementSpeed;
        AttackSpeed = attackSpeed;
        Damage = damage;
    }
    
    public static bool operator ==(Stats a, Stats b)
    {
        return Math.Abs(a.Hp - b.Hp) < TOLERANCE && 
               Math.Abs(a.MovementSpeed - b.MovementSpeed) < TOLERANCE && 
               Math.Abs(a.AttackSpeed - b.AttackSpeed) < TOLERANCE &&
               Math.Abs(a.Damage - b.Damage) < TOLERANCE;
    }

    public static bool operator !=(Stats a, Stats b)
    {
        return !(a == b);
    }
}
