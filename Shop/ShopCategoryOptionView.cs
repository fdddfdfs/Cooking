using UnityEngine;
using UnityEngine.UI;

public class ShopCategoryOptionView : MonoBehaviour
{
    [SerializeField] private Button _option;
    [SerializeField] private Image _icon;

    public void Init(Shop shop, ShopCategoryData shopCategoryData)
    {
        _icon.sprite = shopCategoryData.Sprite;
        _option.onClick.AddListener(() => shop.ChangeCategory(shopCategoryData));
    }
}