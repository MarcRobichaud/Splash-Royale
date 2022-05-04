using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    public List<Tower> hostTowers = new List<Tower>();
    public List<Tower> clientTowers = new List<Tower>();
    public List<Unit> hostUnits = new List<Unit>();
    public List<Unit> clientUnits = new List<Unit>();

    public bool IsIdHost(ulong id) => id == hostStats.Value.ID;
    
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

    public void Init(ulong id, bool isHost)
    {
        InitPlayerStats(id, isHost);
        InitTowers(id, isHost);
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
            tower.owner = id;
        }
    }
}
