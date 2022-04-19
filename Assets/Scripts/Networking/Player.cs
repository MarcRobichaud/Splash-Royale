using Unity.Netcode;

public class Player : NetworkBehaviour
{
    private UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (IsOwner)
        {
            InitPlayerStatsServerRpc(OwnerClientId, IsHost);
        }
    }

    [ServerRpc]
    private void InitPlayerStatsServerRpc(ulong id, bool isHost)
    {
        uiManager.InitPlayerStats(id, isHost);
    }

    private void Spawn()
    {
    }
}
