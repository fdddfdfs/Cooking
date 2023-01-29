using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerItems : IUpdatable
{
    private readonly Items _items;
    private readonly IReadOnlyList<KeyControl> _digitsKeys;
    private Inventory _inventory;

    private IShowable _showableItem;
    private IUpdatable _updatableItem;
    private IUsable _usableItem;
    
    public PlayerItems(
        Building building,
        Inventory inventory,
        ItemData trapData,
        ItemData turretData,
        IEnemiesCollection enemiesCollection)
    {
        _items = new Items(building, inventory, trapData, turretData, enemiesCollection);
        
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
                ChangeCurrentItem(_inventory.GetHotspotItem(i).ItemType);
                return;
            }
        }
    }

    private void ChangeCurrentItem(ItemType itemType)
    {
        HideItem();
        
        Item newItem = _items.AllItems[itemType];
        
        _showableItem = newItem as IShowable;
        _updatableItem = newItem as IUpdatable;
        _usableItem = newItem as IUsable;
        
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