using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Priority SO")]
public class PrioritySO : ScriptableObject
{
    public float detectionRange;
    private float SqrDetectionRange => detectionRange * detectionRange;

    private bool IsInDetectionRange(Vector3 position, Vector3 targetPosition) =>
        Vector3.SqrMagnitude(position - targetPosition) <= SqrDetectionRange;
    
    private ulong ID;
    private List<Tower> Towers => GameManager.Instance.IsIdHost(ID) ? GameManager.Instance.clientTowers : GameManager.Instance.hostTowers;
    private List<Unit> Units => GameManager.Instance.IsIdHost(ID) ? GameManager.Instance.clientUnits : GameManager.Instance.hostUnits;
    

    public void Init(ulong id)
    {
        ID = id;
    }

    public Vector3 GetTarget(Vector3 position)
    {
        Unit target = GetClosest(Units, position);

        if (target != null && IsInDetectionRange(position, target.transform.position))
            return target.transform.position;
        
        return GetClosest(Towers, position).transform.position;
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