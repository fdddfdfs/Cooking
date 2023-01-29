using System;

public class Turret : BuildableItem
{
    protected override string BuildingResourceName => "Turret";

    private readonly IEnemiesCollection _enemiesCollection;
    
    public Turret(Building building, Inventory inventory, ItemData itemData, IEnemiesCollection enemiesCollection)
        : base(building, inventory, itemData)
    {
        _enemiesCollection = enemiesCollection; 
    }
    
    protected override void InstantiateNewBuilding()
    {
        base.InstantiateNewBuilding();
        
        if(_buildableItem.TryGetComponent(out TurretBuilding trapBuilding))
        {
            trapBuilding.Init(_enemiesCollection);
        }
        else
        {
            throw new Exception(
                $"Resource {BuildingResourceName} must contain component {nameof(TurretBuilding)}");
        }
    }
}