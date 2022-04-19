using System;

[Serializable]
public struct Stats
{
    public float Hp;
    public float MovementSpeed;
    public float AttackSpeed;
    public float Damage;

    public Stats(float hp, float movementSpeed = 1f, float attackSpeed = 1f, float damage = 1f)
    {
        Hp = hp;
        MovementSpeed = movementSpeed;
        AttackSpeed = attackSpeed;
        Damage = damage;
    }
}
