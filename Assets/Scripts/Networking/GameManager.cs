using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private static GameManager instance;

    public static GameManager Instance => instance;

    public List<Tower> hostTowers = new List<Tower>();
    public List<Tower> clientTowers = new List<Tower>();
    public List<Unit> hostUnits = new List<Unit>();
    public List<Unit> clientUnits = new List<Unit>();

    public bool IsIdHost(ulong id) => id == hostStats.Value.ID;
    
    public List<Tower> GetTowers(ulong id) => IsIdHost(id) ? hostTowers : clientTowers;
    public List<Unit> GetUnits(ulong id) => IsIdHost(id) ? hostUnits : clientUnits;
    public List<Tower> GetOpponentTowers(ulong id) => IsIdHost(id) ? clientTowers : hostTowers;
    public List<Unit> GetOpponentUnits(ulong id) => IsIdHost(id) ? clientUnits : hostUnits;

    public PlayerStats GetStats(ulong id) => IsIdHost(id) ? HostStats : ClientStats;
    private NetworkVariable<PlayerStats> GetNetworkStats(ulong id) => IsIdHost(id) ? hostStats : clientStats;

    [SerializeField]
    private NetworkVariable<PlayerStats> hostStats = new NetworkVariable<PlayerStats>();
    [SerializeField]
    private NetworkVariable<PlayerStats> clientStats = new NetworkVariable<PlayerStats>();
    public PlayerStats HostStats => hostStats.Value;
    public PlayerStats ClientStats => clientStats.Value;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } 
        else 
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (IsHost)
            ManaManager.Instance.TryRegenerateMana(HostStats, ClientStats);
    }

    public void Init(ulong id, bool isHost)
    {
        if (IsHost)
        {
            InitPlayerStats(id, isHost);
            InitTowers(id, isHost);

            if (NetworkManager.ConnectedClients.Count == 2)
            {
                List<Cards> cardsList = new List<Cards> {Cards.Paladin, Cards.Witch, Cards.Archer, Cards.Healer};
            
                Pool.Instance.Init(cardsList);
                ManaManager.Instance.Init();
            }
        }
    }

    private void InitPlayerStats(ulong id, bool isHost)
    {
        PlayerStats stats = new PlayerStats(id);

        if (isHost)
            hostStats.Value = stats;
        else
            clientStats.Value = stats;
    }

    private void InitTowers(ulong id, bool isHost)
    {
        var towers = isHost ? hostTowers : clientTowers;

        foreach (var tower in towers)
        {
            tower.Init(id);
        }
    }

    public void UpdateStats(PlayerStats stats)
    {
        if (IsHost)
        {
            GetNetworkStats(stats.ID).Value = stats;
        }
    }

    public void TowerDeath(Tower tower)
    {
        GetTowers(tower.ID).Remove(tower);
        
        PlayerStats host = HostStats;
        PlayerStats client = ClientStats;
        
        if (IsIdHost(tower.ID))
        {
            host.Lives -= tower.tower.points;
            client.Score += tower.tower.points;
        }
        else
        {
            client.Lives -= tower.tower.points;
            host.Score += tower.tower.points;
        }
        
        UpdateStats(host);
        UpdateStats(client);
        
        if (host.Lives <= 0 || client.Lives <= 0)
            GameOver();
    }

    private void GameOver()
    {
        for (int i = hostUnits.Count - 1; i >= 0; i--)
        {
            hostUnits[i].OnDeath.Invoke();
        }

        for (int i = clientUnits.Count - 1; i >= 0; i--)
        {
            clientUnits[i].OnDeath.Invoke();
        }

        Spawner.Instance.isGameOver = true;
        GameOverClientRpc();
    }

    [ClientRpc]
    private void GameOverClientRpc()
    {
        FindObjectOfType<UIManager>().gameOver.gameObject.SetActive(true);
    }
}
