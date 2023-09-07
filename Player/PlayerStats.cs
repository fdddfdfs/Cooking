public class PlayerStats
{
    private int _balance;

    public PlayerStats(int startBalance)
    {
        _balance = startBalance;
    }

    public bool TryChangeBalance(int delta)
    {
        if (_balance + delta < 0) return false;

        _balance += delta;

        return true;
    }
}