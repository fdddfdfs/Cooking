using System.Collections.Generic;

public class Items
{
    public Dictionary<ItemType, Item> AllItems { get; }

    public Items(Building building, Inventory inventory, InventoryItemData trapData, InventoryItemData turretData)
    {
        AllItems = new Dictionary<ItemType, Item>
        {
            [ItemType.Trap] = new Trap(building, inventory, trapData),
            [ItemType.Turret] = new Turret(building, inventory, turretData),
        };
    }
}