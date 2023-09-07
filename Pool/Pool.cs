using System.Collections.Generic;

public abstract class Pool<TItem>
{
    private bool _shouldExpand;
    private List<TItem> _items;

    public Pool(bool shouldExpand)
    {
        _shouldExpand = shouldExpand;
    }

    public virtual TItem GetItem()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (IsAvailable(_items[i]))
                return _items[i];
        }

        if (_shouldExpand)
        {
            var item = CreateItem();
            AddItem(item);
            return item;
        }
        else
        {
            throw new System.Exception("no objects for pool");
        }
    }

    protected void Init(int startSize)
    {
        _items = new List<TItem>(startSize);

        for (int i = 0; i < startSize; i++)
        {
            AddItem(CreateItem());
        }
    }

    private void AddItem(TItem item)
    {
        _items.Add(item);
    }

    protected abstract bool IsAvailable(TItem pooledObject);
    
    protected abstract TItem CreateItem();
}
