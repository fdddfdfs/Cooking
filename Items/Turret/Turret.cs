using System;
using UnityEngine;

public class Turret : BuildableItem
{
    private const string AmmoResourceName = "ExplosiveAmmo";
    
    private readonly IEnemiesCollection _enemiesCollection;
    private readonly Inventory _inventory;
    private readonly ItemData _ammoData;
    private readonly TurretView _turretView;
    private readonly Pool<GameObject> _ammoPool;

    private TurretBuilding _turretBuilding;

    public Turret(
        Building building,
        Inventory inventory,
        ItemData itemData,
        IEnemiesCollection enemiesCollection,
        TurretView turretView,
        ItemData ammoData)
        : base(building, inventory, itemData)
    {
        _enemiesCollection = enemiesCollection;
        _inventory = inventory;
        _ammoData = ammoData;
        _turretView = turretView;
        _ammoPool = new FromPrefabPool(ResourcesLoader.LoadObject(AmmoResourceName), 10);
    }
    
    protected override string BuildingResourceName => "Turret";
    
    protected override void InstantiateNewBuilding()
    {
        if (_turretBuilding)
        {
            _turretBuilding.Activate();
        }
        
        base.InstantiateNewBuilding();
        
        if(_buildableItem.TryGetComponent(out TurretBuilding turretBuilding))
        {
            turretBuilding.Init(_enemiesCollection, _turretView, _inventory, _ammoData, _ammoPool);
            _turretBuilding = turretBuilding;
        }
        else
        {
            throw new Exception(
                $"Resource {BuildingResourceName} must contain component {nameof(TurretBuilding)}");
        }
    }
}