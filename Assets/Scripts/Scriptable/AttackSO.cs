using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack SO")]
public class AttackSO : ScriptableObject
{
    public StatsSO stats;
    public float range;
    public float time;
}
