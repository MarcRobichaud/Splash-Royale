public struct PlayerStats
{
    private const int DefaultScore = 0;
    private const int DefaultMana = 10;
    private const int DefaultLives = 3;

    public ulong ID { get; }
    public int Score { get; private set; }
    public int Mana { get; private set; }
    public int Lives { get; private set; }

    public PlayerStats(ulong userID)
    {
        ID = userID;
        Score = DefaultScore;
        Mana = DefaultMana;
        Lives = DefaultLives;
    }
}
