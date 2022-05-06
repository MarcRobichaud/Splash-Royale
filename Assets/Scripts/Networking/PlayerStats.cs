using System;
using Unity.Netcode;

[Serializable]
public struct PlayerStats : INetworkSerializeByMemcpy
{
    private const int DefaultScore = 0;
    private const int DefaultMana = 10;
    private const int DefaultLives = 3;

    public ulong ID;
    public int Score;
    public int Mana;
    public int Lives;

    public PlayerStats(ulong userID)
    {
        ID = userID;
        Score = DefaultScore;
        Mana = DefaultMana;
        Lives = DefaultLives;
    }

    public override string ToString()
    {
        return "ID: " + ID + '\n' +
               "Score: " + Score + '\n' +
               "Mana: " + Mana + '\n' +
               "Lives: " + Lives + '\n';
    }
}
