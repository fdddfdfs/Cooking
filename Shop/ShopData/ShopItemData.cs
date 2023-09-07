using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Data/ShopItem")]
public class ShopItemData : ItemData
{
    [SerializeField] private int _price;
    [SerializeField] private ShopCategoryType _shopCategoryType;

    public int Price => _price;

    public ShopCategoryType ShopCategoryType => _shopCategoryType;
}