using UnityEngine;

public class Spawner
{
    private static Spawner instance = null;
    public static Spawner Instance => instance ??= new Spawner();

    private Spawner()
    {
    }

    public void Spawn(ulong id, Vector3 position, Cards card)
    {
        if (Pool.Instance.IsInit)
        {
            int cost = Pool.Instance.CardSos[card].manaCost;
            PlayerStats pStats = GameManager.Instance.GetStats(id);

            bool isInRange = GameManager.Instance.IsIdHost(id) ? position.z > 0 : position.z < 0;
            
            if (isInRange && ManaManager.Instance.TryBuy(pStats, cost))
            {
                Unit unit = Pool.Instance.GetNewUnit(card, position, id);
                GameManager.Instance.GetUnits(id).Add(unit);
            }
        }
        else
        {
            Debug.Log("Pool not initialize");
        }
    }
}
