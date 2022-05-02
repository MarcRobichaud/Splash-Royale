using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private GameObject go;
    
    private void Start()
    {
        if (IsOwner)
        {
            go = Resources.Load<GameObject>("prefabs/cube");
            InitPlayerStatsServerRpc(OwnerClientId, IsHost);
        }
    }

    [ServerRpc]
    private void InitPlayerStatsServerRpc(ulong id, bool isHost)
    {
        GameManager.Instance.Init(id, isHost);
    }

    private void Update()
    {
        if (IsOwner && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
  
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                SpawnServerRpc(OwnerClientId, hit.transform.position);
            }
        }
    }

    [ServerRpc]
    private void SpawnServerRpc(ulong id, Vector3 position)
    {
        GameObject nobj = Instantiate(go, position, Quaternion.identity);
        nobj.GetComponent<NetworkObject>().Spawn();
        Unit unit = nobj.GetComponent<Unit>();
        unit.Init(id);
    }
}
