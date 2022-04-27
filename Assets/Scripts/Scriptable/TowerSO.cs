using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObjects/Tower SO")]
public class TowerSO : ScriptableObject
{
    public StatsSO stats;
    public GraphicSO graphics;
    public List<AttackSO> attacks;
    public int points;
}
