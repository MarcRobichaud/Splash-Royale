using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Unit SO")]
public class UnitSO : ScriptableObject
{
    public StatsSO stats;
    public MovementSO movement;
    public List<AttackSO> attacks;
    public PrioritySO priority;

    [SerializeField] 
    private Cards card;

    public Cards Card => card;

    public void Init()
    {
        stats = GameObject.Instantiate(stats);
        movement = GameObject.Instantiate(movement);
        priority = GameObject.Instantiate(priority);
        
        for (int i = 0; i < attacks.Count; i++)
        {
            attacks[i] = GameObject.Instantiate(attacks[i]);
        }
        stats.Init();
    }
}
