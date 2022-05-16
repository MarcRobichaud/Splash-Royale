using System.Collections.Generic;
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
        if (isAOE)
        {
            List<IHitable> targets = GetTargetsInAOE(target);
            
            foreach (var t in targets)
            {
                Hit(t);
            }
        }
        else
        {
            Hit(target);
        }
    }

    private void Hit(IHitable target)
    {
        if (duration <= 0)
            target.OnHit(stats.value);
        else 
            target.OnEffectHit(new Effect(stats.value, Time.time, duration));
    }

    public List<IHitable> GetTargetsInAOE(IHitable target)
    {
        List<Unit> units = GameManager.Instance.GetUnits(target.ID);
        List<Tower> towers = GameManager.Instance.GetTowers(target.ID);

        List<IHitable> targets = GetTargetInRange(units, target);
        GetTargetInRange(towers, target, targets);
        
        return targets;
    }

    private List<IHitable> GetTargetInRange<T>(List<T> possibleTargets, IHitable target, List<IHitable> targets = null) where T : IHitable
    {
        if (targets == null)
            targets = new List<IHitable>();

        foreach (var t in possibleTargets)
        {
            if (target.transform.position.IsDistanceFromTargetInRange(t.transform.position, AOERange))
            {
                targets.Add(t);
            }
        }

        return targets;
    }
}
