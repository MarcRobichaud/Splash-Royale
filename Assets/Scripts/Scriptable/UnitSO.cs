using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Unit SO")]
public class UnitSO : ScriptableObject
{
    public StatsSO stats;
    public MovementSO movement;
    public GraphicSO graphics;
    public List<AttackSO> attacks;
    public PrioritySO priority;

    public void Init()
    {
        stats = GameObject.Instantiate(stats);
        movement = GameObject.Instantiate(movement);
        priority = GameObject.Instantiate(priority);
    }
}
