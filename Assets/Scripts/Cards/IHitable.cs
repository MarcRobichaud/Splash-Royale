using UnityEngine;

public interface IHitable
{
    public ulong ID { get; }

    public delegate void Death();

    public Death OnDeath { get; set; }
    public Transform transform { get; }
    public void OnHit(Stats stats);

    public void OnEffectHit(Effect effect);
}
