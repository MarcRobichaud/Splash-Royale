using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spawner
{
    private static Spawner instance = null;

    private GameObject unit1;
    
    List<Unit> Units(ulong id) =>   GameManager.Instance.IsIdHost(id) ? GameManager.Instance.hostUnits : GameManager.Instance.clientUnits;
    private Spawner()
    {
        unit1 = Resources.Load<GameObject>("prefabs/Paladin");
    }

    public static Spawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Spawner();
            }
            return instance;
        }
    }

    public void Spawn(ulong id, Vector3 position)
    {
        GameObject nobj = GameObject.Instantiate(unit1, position, Quaternion.identity);
        nobj.GetComponent<NetworkObject>().Spawn();
        Unit unit = nobj.GetComponent<Unit>();
        unit.Init(id);
        
        Units(id).Add(unit);
    }
}
