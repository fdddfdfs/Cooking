using System;
using UnityEngine;

public class Inventory
{
    private const string InventoryViewResourceName = "InventoryView";
    private const int InventoryGearSize = 4;
    private const int InventoryMainSize = 36;
    
    private readonly InventoryView _inventoryView;

    private readonly InventoryCell[] _inventory;
    private readonly InventoryCell[] _gear;
    
    public Inventory(Canvas canvas)
    {
        _inventoryView = ResourcesLoader.InstantiateLoadComponent<InventoryView>(InventoryViewResourceName);
        _inventoryView.Init(this, InventoryGearSize, InventoryMainSize);
        _inventoryView.transform.SetParent(canvas.transform, false);

        _inventory = CreateInventoryCells(InventoryMainSize, 0 , ItemType.Unspecified);
        _gear = CreateInventoryCells(
            InventoryGearSize,
            InventoryMainSize,
            new[] { ItemType.Head, ItemType.Chest, ItemType.Legs, ItemType.Boots });
    }
    
    public int AddItem(InventoryItemData item, int count)
    {
        int firstEmptyIndex = -1;
        
        for (var i = 0; i < InventoryMainSize; i++)
        {
            if (_inventory[i].IsEquals(item))
            {
                if (!_inventory[i].IsFull())
                {
                    count = _inventory[i].IncreaseCount(count);

                    if (count == 0)
                    {
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
            return 0;
        }

        return count;
    }

    public bool RemoveItem(InventoryItemData item, int count)
    {
        if (!IsEnoughItems(item, count)) return false;

        for (int i = InventoryMainSize - 1; i >= 0; i--)
        {
            if (_inventory[i].IsEquals(item))
            {
                count = _inventory[i].DecreaseCount(count);

                if (count == 0)
                {
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
        InventoryItemData itemDataFrom = fromCell.ItemData;
        
        fromCell.ChangeItemData(toCell.ItemData, toCell.Count);
        toCell.ChangeItemData(itemDataFrom, countFrom);
    }

    private bool IsEnoughItems(InventoryItemData item, int count)
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
        for (int i = 0; i < count; i++)
        {
            inventoryCells[i] = new InventoryCell(i + indexOffset, _inventoryView, cellType);
        }

        return inventoryCells;
    }
    
    private InventoryCell[] CreateInventoryCells(int count, int indexOffset, ItemType[] cellTypes)
    {
        var inventoryCells = new InventoryCell[count];
        for (int i = 0; i < count; i++)
        {
            inventoryCells[i] = new InventoryCell(i + indexOffset, _inventoryView, cellTypes[i]);
        }

        return inventoryCells;
    }

    private InventoryCell GetCell(int index)
    {
        if (index >= InventoryMainSize)
        {
            return _gear[index - InventoryMainSize];
        }
        
        return _inventory[index];
    }
}
