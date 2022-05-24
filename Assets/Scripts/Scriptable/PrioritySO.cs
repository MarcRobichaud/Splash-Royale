using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Priority SO")]
public class PrioritySO : ScriptableObject
{
    public float detectionRange;
    public bool isHealer;
    
    private List<Unit> GetUnits => !isHealer ? GameManager.Instance.GetOpponentUnits(ID) : GetUnitsWithoutHealer();

    private ulong ID;
    
    public void Init(ulong id)
    {
        ID = id;
    }

    public IHitable GetTarget(Vector3 position)
    {
        Unit target = GetClosest(GetUnits, position);

        if (target != null &&  position.IsDistanceFromTargetInRange(target.transform.position, detectionRange))
            return target;
        
        return GetClosest(GameManager.Instance.GetOpponentTowers(ID), position);
    }

    private List<Unit> GetUnitsWithoutHealer()
    {
        List<Unit> units = new List<Unit>();
        
        foreach (var unit in GameManager.Instance.GetUnits(ID))
        {
            if (!unit.unitSO.priority.isHealer)
                units.Add(unit);
        }

        return units;
    }

    private T GetClosest<T>(List<T> list, Vector3 position) where T : MonoBehaviour
    {
        if (list == null || list.Count == 0) return null;
        
        float closestDistance = Vector3.SqrMagnitude(position - list[0].transform.position);
        T target = list[0];
        
        for (int i = 1; i < list.Count; i++)
        {
            float currentDistance = Vector3.SqrMagnitude(position - list[i].transform.position);
            
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                target = list[i];
            }
        }

        return target;
    }
}
