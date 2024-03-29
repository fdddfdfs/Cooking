using System;
using UnityEngine;

public class Inventory : MenuWithView<InventoryView>
{
    private const string InventoryViewResourceName = "InventoryView";
    private const int InventoryGearSize = 4;
    private const int InventoryMainSize = 36;
    private const int InventoryHotspotSize = 9;
    private const int InventoryHotspotStartIndex = InventoryMainSize - InventoryHotspotSize;

    private readonly InventoryCell[] _inventory;
    private readonly InventoryCell[] _gear;

    public event Action InventoryChanged; 

    public Inventory(Canvas canvas) : base(canvas, InventoryViewResourceName)
    {
        _view.Init(this, InventoryGearSize, InventoryMainSize);

        _inventory = CreateInventoryCells(InventoryMainSize, 0 , ItemType.Unspecified);
        _gear = CreateInventoryCells(
            InventoryGearSize,
            InventoryMainSize,
            new[] { ItemType.Head, ItemType.Chest, ItemType.Legs, ItemType.Boots });
    }

    public int AddItem(ItemData item, int count)
    {
        int firstEmptyIndex = -1;
        int initialCount = count;
        
        for (var i = 0; i < InventoryMainSize; i++)
        {
            if (_inventory[i].IsEquals(item))
            {
                if (!_inventory[i].IsFull())
                {
                    count = _inventory[i].IncreaseCount(count);

                    if (count == 0)
                    {
                        InventoryChanged?.Invoke();
                        return 0;
                    }
                }
            }
            else if (_inventory[i].ItemData == null && firstEmptyIndex == -1)
            {
                firstEmptyIndex = i;
            }
        }

        if (firstEmptyIndex != -1)
        {
            _inventory[firstEmptyIndex].ChangeItemData(item, count);
            InventoryChanged?.Invoke();
            return 0;
        }

        if (count != initialCount)
        {
            InventoryChanged?.Invoke();
        }
        
        return count;
    }

    public bool RemoveItem(ItemData item, int count)
    {
        if (!IsEnoughItems(item, count)) return false;

        for (int i = InventoryMainSize - 1; i >= 0; i--)
        {
            if (_inventory[i].IsEquals(item))
            {
                count = _inventory[i].DecreaseCount(count);

                if (count == 0)
                {
                    InventoryChanged?.Invoke();
                    return true;
                }
            }
        }

        throw new Exception($"IsEnoughItems return true, but {count} item count left");
    }

    public void SwapItems(int fromIndex, int toIndex)
    {
        InventoryCell fromCell = GetCell(fromIndex);
        InventoryCell toCell = GetCell(toIndex);

        if (toCell.ItemData != null && fromCell.IsRestricted(toCell.ItemData.ItemType) ||
            fromCell.ItemData != null && toCell.IsRestricted(fromCell.ItemData.ItemType))
        {
            return;
        }
        
        int countFrom = fromCell.Count;
        ItemData itemDataFrom = fromCell.ItemData;
        
        fromCell.ChangeItemData(toCell.ItemData, toCell.Count);
        toCell.ChangeItemData(itemDataFrom, countFrom);

        InventoryChanged?.Invoke();
    }

    public ItemData GetHotspotItem(int index)
    {
        return _inventory[InventoryHotspotStartIndex + index].ItemData;
    }

    public void ShowSelectedItem(int index)
    {
        _view.UpdateSelectedItem(GetCell(index).ItemData);
    }

    private bool IsEnoughItems(ItemData item, int count)
    {
        for (int i = InventoryMainSize - 1; i >= 0; i--)
        {
            if (_inventory[i].IsEquals(item))
            {
                count -= _inventory[i].Count;

                if (count <= 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private InventoryCell[] CreateInventoryCells(int count, int indexOffset, ItemType cellType)
    {
        var inventoryCells = new InventoryCell[count];
        for (var i = 0; i < count; i++)
        {
            inventoryCells[i] = new InventoryCell(i + indexOffset, _view, cellType);
        }

        return inventoryCells;
    }
    
    private InventoryCell[] CreateInventoryCells(int count, int indexOffset, ItemType[] cellTypes)
    {
        if (count != cellTypes.Length)
        {
            throw new Exception("Count and cellTypes.Length must be equal");
        }
        
        var inventoryCells = new InventoryCell[count];
        for (var i = 0; i < count; i++)
        {
            inventoryCells[i] = new InventoryCell(i + indexOffset, _view, cellTypes[i]);
        }

        return inventoryCells;
    }

    private InventoryCell GetCell(int index)
    {
        return index >= InventoryMainSize ? _gear[index - InventoryMainSize] : _inventory[index];
    }
}
