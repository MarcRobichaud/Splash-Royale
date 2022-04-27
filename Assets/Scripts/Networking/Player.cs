using Unity.Netcode;

public class Player : NetworkBehaviour
{
    private void Start()
    {
        if (IsOwner)
        {
            InitPlayerStatsServerRpc(OwnerClientId, IsHost);
        }
    }

    [ServerRpc]
    private void InitPlayerStatsServerRpc(ulong id, bool isHost)
    {
        GameManager.Instance.Init(id, isHost);
    }

    private void Spawn()
    {
    }
}
