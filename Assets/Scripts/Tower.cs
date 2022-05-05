using Unity.Netcode;
using UnityEngine;

public class Tower : NetworkBehaviour
{
    private ulong ID;
    public TowerSO tower;

    public void Init(ulong id)
    {
        tower = GameObject.Instantiate(tower);
        tower.Init();
    }
}
    
