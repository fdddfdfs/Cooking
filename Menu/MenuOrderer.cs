using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuOrderer
{
    private readonly Dictionary<Type, Menu> _menus;
    private readonly List<Menu> _menuQueue;

    public Menu this[Type type] => _menus[type];

    public bool IsAnyMenuOpen => _menuQueue.Count != 0;

    public MenuOrderer(
        Canvas canvas,
        List<ShopItemData> shopItemDatas,
        List<ShopCategoryData> shopCategoryDatas,
        IPlayer player)
    {
        _menus = new Dictionary<Type, Menu>
        {
            [typeof(Shop)] = new Shop(canvas, shopItemDatas, shopCategoryDatas, player),
            [typeof(Inventory)] = new Inventory(canvas),
        };

        _menuQueue = new List<Menu>();
    }

    public void SwapMenuActive(Type menu)
    {
        Menu targetMenu = _menus[menu];
        
        bool result = targetMenu.SwapActive();

        if (result)
        {
            _menuQueue.Add(targetMenu);
            targetMenu.SetAsLastSibling();
        }
        else
        {
            _menuQueue.Remove(targetMenu);
        }
    }

    public void CloseUpperMenu()
    {
        if (!IsAnyMenuOpen) return;
        
        bool affectCursor = _menuQueue.Count == 1;
        
        _menuQueue[^1].ChangeMenuActive(false, affectCursor);
        _menuQueue.RemoveAt(_menuQueue.Count - 1);
    }
}