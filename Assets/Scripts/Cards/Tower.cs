using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Tower : NetworkBehaviour, IHitable
{
    private ulong ID;
    public TowerSO tower;
    public Image UIHealth;
    
    [SerializeField]
    private NetworkVariable<Stats> networkStats = new NetworkVariable<Stats>();

    [SerializeField] 
    private NetworkVariable<Stats> initialStats = new NetworkVariable<Stats>();



    public IHitable.Death OnDeath { get; set; }

    public void Init(ulong id)
    {
        tower = GameObject.Instantiate(tower);
        tower.Init();
    }

    private void Update()
    {
        if (IsHost)
        {
            if (networkStats.Value != tower.stats.value)
                networkStats.Value = tower.stats.value;

            if (initialStats.Value != tower.stats.InitialValue)
                initialStats.Value = tower.stats.InitialValue;
        }

        UIHealth.fillAmount = networkStats.Value.Hp / initialStats.Value.Hp;
    }

    public void OnHit(Stats stats)
    {
        tower.stats.Hit(stats);
        
        if(tower.stats.IsDead)
            OnDeath.Invoke();
    }
}
    
