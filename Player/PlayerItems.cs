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
    private readonly MenuOrderer _menuOrderer;

    private IShowable _showableItem;
    private IUpdatable _updatableItem;
    private IUsable _usableItem;
    private int _currentItemIndex;

    public Item CurrentItem { get; private set; }
    
    public ItemData CurrentItemData { get; private set; }

    private Action ChangeInventory => () => ChangeCurrentItem(_currentItemIndex);
    
    public PlayerItems(
        MenuOrderer menuOrderer,
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
        _inventory.InventoryChanged += ChangeInventory;

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

        _menuOrderer = menuOrderer;
    }

    ~PlayerItems()
    {
        _inventory.InventoryChanged -= ChangeInventory;
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
        _currentItemIndex = hotspotIndex;
        
        ItemData hotSpotItem = _inventory.GetHotspotItem(hotspotIndex);

        if (hotSpotItem && !_items.AllItems.ContainsKey(hotSpotItem))
        {
            Debug.LogWarning($"ItemType {hotSpotItem.ItemType} cannot be shown coz that`s not in AllItems");
            SetCurrentItem(null, hotSpotItem);
            return;
        }
        
        SetCurrentItem(hotSpotItem ? _items.AllItems[hotSpotItem] : null, hotSpotItem);
    }

    private void SetCurrentItem(Item item, ItemData itemData)
    {
        HideItem();

        _showableItem = item as IShowable;
        _updatableItem = item as IUpdatable;
        _usableItem = item as IUsable;

        CurrentItem = item;
        CurrentItemData = itemData;
        
        ShowItem();
    }

    private void UseItem()
    {
        if (_menuOrderer.IsAnyMenuOpen)
        {
            return;
        }
        
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