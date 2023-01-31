using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerItems : IUpdatable
{
    private readonly Items _items;
    private readonly IReadOnlyList<KeyControl> _digitsKeys;
    private readonly Inventory _inventory;

    private IShowable _showableItem;
    private IUpdatable _updatableItem;
    private IUsable _usableItem;
    private int _currentItemIndex;

    private Action OnChangeInventory => () => ChangeCurrentItem(_currentItemIndex);
    
    public PlayerItems(
        Building building,
        Inventory inventory,
        ItemData trapData,
        ItemData turretData,
        IEnemiesCollection enemiesCollection,
        Canvas canvas,
        ItemData turretAmmoData)
    {
        _items = new Items(building, inventory, trapData, turretData, enemiesCollection, canvas, turretAmmoData);
        _inventory = inventory;
        _inventory.InventoryChanged += OnChangeInventory;

        _digitsKeys = new List<KeyControl>
        {
            Keyboard.current.digit1Key,
            Keyboard.current.digit2Key,
            Keyboard.current.digit3Key,
            Keyboard.current.digit4Key,
            Keyboard.current.digit5Key,
            Keyboard.current.digit6Key,
            Keyboard.current.digit7Key,
            Keyboard.current.digit8Key,
            Keyboard.current.digit9Key,
        };
    }

    ~PlayerItems()
    {
        _inventory.InventoryChanged -= OnChangeInventory;
    }
    
    public void Update()
    {
        CheckInput();
        UpdateItem();
    }

    private void CheckInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            UseItem();
        }
        
        for (int i = 0; i < _digitsKeys.Count; i++)
        {
            if (_digitsKeys[i].wasPressedThisFrame)
            {
                ChangeCurrentItem(i);
                return;
            }
        }
    }

    private void ChangeCurrentItem(int hotspotIndex)
    {
        ItemData hotSpotItem = _inventory.GetHotspotItem(hotspotIndex);
        SetCurrentItem(hotSpotItem ? _items.AllItems[hotSpotItem.ItemType] : null);
        _currentItemIndex = hotspotIndex;
    }

    private void SetCurrentItem(Item item)
    {
        HideItem();

        _showableItem = item as IShowable;
        _updatableItem = item as IUpdatable;
        _usableItem = item as IUsable;
        
        ShowItem();
    }

    private void UseItem()
    {
        _usableItem?.Use();
    }

    private void UpdateItem()
    {
        _updatableItem?.Update();
    }

    private void HideItem()
    {
        _showableItem?.Hide();
    }

    private void ShowItem()
    {
        _showableItem?.Show();
    }
}