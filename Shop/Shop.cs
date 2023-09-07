using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MenuWithView<ShopView>
{
    private const string ShopViewResourceName = "UI/Shop/Shop";
    private const ShopCategoryType DefaultCategoryType = ShopCategoryType.Defence;
    
    private readonly Dictionary<ShopCategoryType, List<ShopItemData>> _categoryItems;
    private readonly IPlayer _player;
    
    public Shop(
        Canvas canvas,
        List<ShopItemData> shopItemDatas,
        List<ShopCategoryData> shopCategoryDatas,
        IPlayer player) 
        : base(canvas, ShopViewResourceName)
    {
        _view.Init(this, shopCategoryDatas);
        
        _categoryItems = new Dictionary<ShopCategoryType, List<ShopItemData>>();
        foreach (ShopCategoryType categoryType in (ShopCategoryType[]) Enum.GetValues(typeof(ShopCategoryType)))
        {
            _categoryItems[categoryType] = new List<ShopItemData>();
        }

        foreach (ShopItemData shopItemData in shopItemDatas)
        {
            _categoryItems[shopItemData.ShopCategoryType].Add(shopItemData);
        }

        _player = player;
        
        _view.ChangeShopCategory(_categoryItems[DefaultCategoryType]);
    }

    public void BuyItem(ShopItemData item)
    {
        bool result = _player.PlayerStats.TryChangeBalance(item.Price);

        if (result)
        {
            _player.Inventory.AddItem(item, 1);
        }
    }

    public void SelectItem(ShopItemData item)
    {
        _view.UpdateCurrentSelectedItem(item);
    }

    public void ChangeCategory(ShopCategoryData shopCategoryData)
    {
        _view.ChangeShopCategory(_categoryItems[shopCategoryData.Type]);
    }
}