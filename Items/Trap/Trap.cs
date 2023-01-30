using System;

public class Trap : BuildableItem
{
    protected override string BuildingResourceName => "Trap";

    private TrapBuilding _trapBuilding;

    public Trap(Building building, Inventory inventory, ItemData itemData)
        : base(building, inventory, itemData) { }

    protected override void InstantiateNewBuilding()
    {
        if (_trapBuilding)
        {
            _trapBuilding.Activate();
        }

        base.InstantiateNewBuilding();
        
        if(_buildableItem.TryGetComponent(out TrapBuilding trapBuilding))
        {
            _trapBuilding = trapBuilding;
            trapBuilding.Init();
        }
        else
        {
            throw new Exception(
                $"Resource {BuildingResourceName} must contain component {nameof(TrapBuilding)}");
        }
    }
}