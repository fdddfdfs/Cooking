using System;

public class Trap : BuildableItem
{
    protected override string BuildingResourceName => "Trap";

    public Trap(Building building, Inventory inventory, ItemData itemData)
        : base(building, inventory, itemData) { }

    protected override void InstantiateNewBuilding()
    {
        base.InstantiateNewBuilding();
        
        if(_buildableItem.TryGetComponent(out TrapBuilding trapBuilding))
        {
            trapBuilding.Init();
        }
        else
        {
            throw new Exception(
                $"Resource {BuildingResourceName} must contain component {nameof(TrapBuilding)}");
        }
    }
}