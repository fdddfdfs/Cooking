using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    private const string ShopSelectedItemViewResourceName = "UI/Shop/ShopSelectedItem";
    private const string ShopCategoryViewResourceName = "UI/Shop/ShopCategory";

    private ShopSelectedItemView _shopSelectedItemView;
    private ShopCategoryView _shopCategoryView;

    public void Init(Shop shop, List<ShopCategoryData> shopCategoriesData)
    {
        _shopSelectedItemView = ResourcesLoader.LoadMenuView<ShopSelectedItemView>(
            transform,
            ShopSelectedItemViewResourceName);
        _shopSelectedItemView.Init(shop);

        _shopCategoryView = ResourcesLoader.LoadMenuView<ShopCategoryView>(transform, ShopCategoryViewResourceName);
        _shopCategoryView.Init(shop, shopCategoriesData);
    }

    public void UpdateCurrentSelectedItem(ShopItemData shopItemData)
    {
        _shopSelectedItemView.UpdateSelectedItem(shopItemData);
    }

    public void ChangeShopCategory(List<ShopItemData> shopItemDatas)
    {
        _shopCategoryView.ChangeCategoryItems(shopItemDatas);
    }
}