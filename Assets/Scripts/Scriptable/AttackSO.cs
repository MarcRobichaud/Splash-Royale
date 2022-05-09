using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack SO")]
public class AttackSO : ScriptableObject
{
    public StatsSO stats;
    public float range;
    public float cooldown;
    public float duration; //for damage overtime
    public bool isAOE;
    public float AOERange;

    private float timeStarted;
    public bool IsCooldownOver => Time.time - timeStarted > cooldown;

    public void Attack(IHitable target)
    {
        timeStarted = Time.time;
        target.OnHit(stats.value);
    }
}
