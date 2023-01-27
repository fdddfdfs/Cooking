public class InventoryCell
{
    private readonly int _index;
    private readonly InventoryView _inventoryView;

    private InventoryItemData _itemData;
    
    public int Count { get; private set; }

    public InventoryCell(int index, InventoryView inventoryView)
    {
        Clear();
        _index = index;
        _inventoryView = inventoryView;
    }

    public void ChangeItemData(InventoryItemData newData, int newCount)
    {
        _itemData = newData;
        Count = newCount;
        UpdateView();
    }

    public int IncreaseCount(int addingCount)
    {
        int newValue = Count + addingCount;

        Count = newValue > _itemData.MaxStack ? _itemData.MaxStack : newValue;
        UpdateView();
        
        return newValue - Count;
    }

    public int DecreaseCount(int decreasingCount)
    {
        int newValue = Count - decreasingCount;
        if (newValue <= 0)
        {
            Clear();
            return newValue * -1;
        } 
        
        Count = newValue;
        UpdateView();
        return 0;
    }

    public bool IsFull()
    {
        return _itemData.MaxStack == Count;
    }

    public bool IsEquals(InventoryItemData inventoryItemData)
    {
        return inventoryItemData.Name == _itemData.Name;
    }

    private void Clear()
    {
        Count = 0;
        _itemData = null;
        UpdateView();
    }

    private void UpdateView()
    {
        _inventoryView.UpdateItem(_index, Count, _itemData);
    }
}