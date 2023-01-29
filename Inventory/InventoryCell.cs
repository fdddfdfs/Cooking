using UnityEngine;

public class InventoryCell
{
    private readonly int _index;
    private readonly InventoryView _inventoryView;
    
    public ItemType CellRestriction { get; private set; }
    
    public ItemData ItemData { get; private set; }
    
    public int Count { get; private set; }

    public InventoryCell(int index, InventoryView inventoryView, ItemType cellRestriction)
    {
        _inventoryView = inventoryView;
        Clear();
        _index = index;
        CellRestriction = cellRestriction;
    }

    public void ChangeItemData(ItemData newData, int newCount)
    {
        ItemData = newData;
        Count = newCount;
        UpdateView();
    }

    public int IncreaseCount(int addingCount)
    {
        int newValue = Count + addingCount;

        Count = newValue > ItemData.MaxStack ? ItemData.MaxStack : newValue;
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
        return ItemData.MaxStack == Count;
    }

    public bool IsEquals(ItemData itemData)
    {
        if (ItemData == null) return false;
        
        return itemData.Name == ItemData.Name;
    }

    public bool IsRestricted(ItemType itemType)
    {
        return (itemType & CellRestriction) == 0;
    }

    private void Clear()
    {
        Count = 0;
        ItemData = null;
        UpdateView();
    }

    private void UpdateView()
    {
        _inventoryView.UpdateItem(_index, Count, ItemData);
    }
}