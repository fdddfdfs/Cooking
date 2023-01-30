using System;
using UnityEngine;

public class Turret : BuildableItem
{
    protected override string BuildingResourceName => "Turret";

    private readonly IEnemiesCollection _enemiesCollection;

    private TurretBuilding _turretBuilding;

    public Turret(Building building, Inventory inventory, ItemData itemData, IEnemiesCollection enemiesCollection)
        : base(building, inventory, itemData)
    {
        _enemiesCollection = enemiesCollection;
    }
    
    protected override void InstantiateNewBuilding()
    {
        if (_turretBuilding)
        {
            _turretBuilding.Activate();
        }
        
        base.InstantiateNewBuilding();
        
        if(_buildableItem.TryGetComponent(out TurretBuilding turretBuilding))
        {
            turretBuilding.Init(_enemiesCollection);
            _turretBuilding = turretBuilding;
        }
        else
        {
            throw new Exception(
                $"Resource {BuildingResourceName} must contain component {nameof(TurretBuilding)}");
        }
    }
}