public interface Factory<out TItem, in TKey>
{
    public abstract TItem GetItem(TKey key);
}