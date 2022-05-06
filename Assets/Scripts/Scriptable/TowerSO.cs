using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObjects/Tower SO")]
public class TowerSO : ScriptableObject
{
    public StatsSO stats;
    public GraphicSO graphics;
    public List<AttackSO> attacks;
    public int points;

    public void Init()
    {
        stats = GameObject.Instantiate(stats);
        for (int i = 0; i < attacks.Count; i++)
        {
            attacks[i] = GameObject.Instantiate(attacks[i]);
        }
    }
}
