using System;

public class Inventory
{
    private const string InventoryViewResourceName = "InventoryView";
    private const int InventoryGearSize = 4;
    private const int InventoryMainSize = 36;
    
    private readonly InventoryView _inventoryView;

    private readonly InventoryCell[] _inventory;
    private readonly InventoryCell[] _gear;
    
    public Inventory()
    {
        _inventoryView = ResourcesLoader.InstantiateLoadComponent<InventoryView>(InventoryViewResourceName);
        _inventoryView.Init(InventoryGearSize, InventoryMainSize);

        _inventory = CreateInventoryCells(InventoryMainSize, 0 );
        _gear = CreateInventoryCells(InventoryGearSize, InventoryMainSize);
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
            else if (_inventory[i] == null && firstEmptyIndex == -1)
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

    private InventoryCell[] CreateInventoryCells(int count, int indexOffset)
    {
        var inventoryCells = new InventoryCell[count];
        for (int i = 0; i < count; i++)
        {
            inventoryCells[i] = new InventoryCell(i + indexOffset, _inventoryView);
        }

        return inventoryCells;
    }
}
