using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Priority SO")]
public class PrioritySO : ScriptableObject
{
    private ulong ID;
    private List<Tower> towers;

    public void Init(ulong id)
    {
        ID = id;
        GetTargetTowers();
    }

    private void GetTargetTowers()
    {
        towers = ID == GameManager.Instance.HostStats.ID ? GameManager.Instance.clientTowers : GameManager.Instance.hostTowers;
    }

    public Vector3 GetTarget(Vector3 position)
    {
        if (towers == null) return new Vector3();
        
        float closestDistance = Vector3.Distance(position, towers[0].transform.position);
        Vector3 target = towers[0].transform.position;

        for (int i = 1; i < towers.Count; i++)
        {
            float currentDistance = Vector3.Distance(position, towers[i].transform.position);
            
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                target = towers[i].transform.position;
            }
        }

        return target;
    }
}
