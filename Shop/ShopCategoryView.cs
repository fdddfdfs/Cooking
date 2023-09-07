using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopCategoryView : MonoBehaviour
{
    private const int ShopItemsCount = 20;
    private const string ShopCategoryOptionViewPrefabResourceName = "UI/Shop/ShopCategoryOption";
    private const string ShopItemViewPrefabResourceName = "UI/Shop/ShopItem";
    
    [SerializeField] private Transform _categoryOptionsParent;
    [SerializeField] private Transform _itemsParent;

    private List<ShopItemView> _items;
    private int _currentItemsCount;
    private Shop _shop;

    public void Init(Shop shop, List<ShopCategoryData> categoryDatas)
    {
        _items = new List<ShopItemView>();
        for (var i = 0; i < ShopItemsCount; i++)
        {
            _items.Add(ResourcesLoader.LoadMenuView<ShopItemView>(_itemsParent, ShopItemViewPrefabResourceName));
        }
        
        foreach (ShopItemView shopItemView in _items)
        {
            shopItemView.Init(shop);
        }

        foreach (ShopCategoryData shopCategoryData in categoryDatas)
        {
            var shopCategoryOptionView = ResourcesLoader.LoadMenuView<ShopCategoryOptionView>(
                _categoryOptionsParent,
                ShopCategoryOptionViewPrefabResourceName);
            shopCategoryOptionView.Init(shop, shopCategoryData);
        }

        _shop = shop;
    }

    public void ChangeCategoryItems(List<ShopItemData> categoryItems)
    {
        if (categoryItems.Count > _items.Count)
        {
            throw new Exception("Items cannot be shown in one window");
        }
        
        for (var i = 0; i < categoryItems.Count; i++)
        {
            _items[i].ChangeItem(categoryItems[i]);
        }

        if (_currentItemsCount > categoryItems.Count)
        {
            for (int i = categoryItems.Count; i < _currentItemsCount; i++)
            {
                _items[i].ChangeItem(null);
            }
        }

        _currentItemsCount = categoryItems.Count;

        if (categoryItems.Count == 0)
        {
            Debug.LogWarning("Shop doesnt have any items in this category");
        }
        else
        {
            _shop.SelectItem(categoryItems[0]);
        }
    }
}