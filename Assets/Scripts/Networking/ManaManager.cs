using UnityEngine;

public class ManaManager
{
    private static ManaManager instance = null;
    public static ManaManager Instance => instance ??= new ManaManager();

    private ManaManager()
    {
    }
    
    private const float ManaRegenerationTime = 5;
    private const int RegeneratedMana = 1;
    private float lastRegenerationTime;

    private bool isInit;

    public void Init()
    {
        isInit = true;
    }

    public bool CanBuy(PlayerStats stats, int manaCost) => stats.Mana >= manaCost;

    public void TryRegenerateMana(PlayerStats stats1, PlayerStats stats2)
    {
        if (isInit && Time.time > lastRegenerationTime + ManaRegenerationTime)
        {
            Debug.Log("Regen Mana");
            lastRegenerationTime = Time.time;
            RegenerateMana(stats1);
            RegenerateMana(stats2);
        }
    }

    private void RegenerateMana(PlayerStats stats)
    {
        stats.RegenMana(RegeneratedMana);
        GameManager.Instance.UpdateStats(stats);
    }

    public bool TryBuy(PlayerStats stats, int cost)
    {
        if (!CanBuy(stats, cost)) 
            return false;
        
        stats.RemoveMana(cost);
        GameManager.Instance.UpdateStats(stats);
        return true;
    }
}
