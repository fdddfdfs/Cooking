using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _emptySprite;
    
    private ShopItemData _currentItemData;
    
    public void Init(Shop shop)
    {
        _button.onClick.AddListener(() => shop.SelectItem(_currentItemData));
        ChangeItem(null);
    }
    
    public void ChangeItem(ShopItemData shopItemData)
    {
        _icon.sprite = shopItemData ? shopItemData.Sprite : _emptySprite;
        _currentItemData = shopItemData;

        _button.enabled = shopItemData;
    }
}