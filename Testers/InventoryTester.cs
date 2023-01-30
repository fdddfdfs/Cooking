using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private ItemData _item;
    [SerializeField] private ItemData _item2;
    [SerializeField] private ItemData _turret;

    private Inventory _inventory;

    private void Awake()
    {
        _inventory = new Inventory(_canvas);
        //_inventory.AddItem(_turret, 1);
    }

    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            _inventory.SwapActive();
        }
    }
}