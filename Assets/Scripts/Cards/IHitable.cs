using UnityEngine;

public interface IHitable
{ 
    public delegate void Death();

    public Death OnDeath { get; set; }
    public Transform transform { get; }
    public void OnHit(Stats stats);
}
