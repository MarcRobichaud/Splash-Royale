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
            Unit unit = Pool.Instance.GetNewUnit(card, position, id);
            GameManager.Instance.GetUnits(id).Add(unit);
        }
    }
}
