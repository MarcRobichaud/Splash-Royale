using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private Cards selected;
    private void Start()
    {
        if (IsOwner)
        {
            InitPlayerStatsServerRpc(OwnerClientId, IsHost);

            UnitSelector selector = FindObjectOfType<UnitSelector>();
            
            if (selector)
                selector.Init(this);
            else
            {
                Debug.Log("unit selector not found");
            }
        }
    }

    public void ChangeSelectedUnit(Cards card)
    {
        if (IsOwner)
            selected = card;
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
            //Doing a raycast to determine the world position of the click
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
  
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                SpawnServerRpc(OwnerClientId, hit.transform.position, selected);
            }
        }
    }

    [ServerRpc]
    private void SpawnServerRpc(ulong id, Vector3 position, Cards card)
    {
        Spawner.Instance.Spawn(id, position, card);
    }
}
