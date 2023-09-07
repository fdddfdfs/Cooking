public interface IPlayer
{
    public Inventory Inventory { get; }

    public PlayerStats PlayerStats { get; }

    public PlayerItems PlayerItems { get; }

    public PlayerMessageView PlayerMessageView { get; }
}