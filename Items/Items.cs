using System.Collections.Generic;
using UnityEngine;

public class Items
{
    private const string TurretViewResourceName = "TurretView";
    
    public Dictionary<ItemData, Item> AllItems { get; }

    public Items(
        Building building,
        Inventory inventory,
        ItemData trapData,
        ItemData turretData,
        IEnemiesCollection enemiesCollection,
        Canvas canvas,
        ItemData turretAmmo)
    {
        var turretView = ResourcesLoader.InstantiateLoadComponent<TurretView>(TurretViewResourceName);
        turretView.transform.SetParent(canvas.transform, false);
        
        AllItems = new Dictionary<ItemData, Item>
        {
            [trapData] = new Trap(building, inventory, trapData),
            [turretData] = new Turret(
                building,
                inventory,
                turretData,
                enemiesCollection,
                turretView,
                turretAmmo),
        };
    }
}