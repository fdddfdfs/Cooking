public class Trap : BuildableItem
{
    protected override string BuildingResourceName => "Trap";

    public Trap(Building building, Inventory inventory, InventoryItemData itemData) 
        : base(building, inventory, itemData) { }
}