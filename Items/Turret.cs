public class Turret : BuildableItem
{
    protected override string BuildingResourceName => "Turret";
    
    public Turret(Building building, Inventory inventory, InventoryItemData itemData) 
        : base(building, inventory, itemData) { }
}