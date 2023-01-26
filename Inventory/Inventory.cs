using System;
using UnityEngine;

public class Inventory
{
    private const string InventoryViewResourceName = "InventoryView";

    private readonly InventoryView _inventoryView;
    
    public Inventory(Transform canvas)
    {
        var inventoryViewObject = Resources.Load(InventoryViewResourceName) as GameObject;
        if (!inventoryViewObject)
        {
            throw new Exception($"Resources folder does not contain {InventoryViewResourceName}");
        }

        inventoryViewObject.transform.parent = canvas;
        _inventoryView = inventoryViewObject.GetComponent<InventoryView>();

        if (!_inventoryView)
        {
            throw new Exception(
                $"{InventoryViewResourceName} resource should contain {nameof(InventoryView)} component");
        }
    }
}
