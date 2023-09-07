using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSelectedItemView : MonoBehaviour
{
    [SerializeField] private Image _selectedItem;
    [SerializeField] private TMP_Text _selectedItemName;
    [SerializeField] private TMP_Text _selectedItemDescription;
    [SerializeField] private TMP_Text _selectedItemPrice;
    [SerializeField] private Button _buy;
    
    private ShopItemData _currentSelectedItemData;

    public void Init(Shop shop)
    {
        _buy.onClick.AddListener(() => shop.BuyItem(_currentSelectedItemData));
    }
    
    public void UpdateSelectedItem(ShopItemData selectedItem)
    {
        _selectedItem.sprite = selectedItem.Sprite;
        _selectedItemName.text = selectedItem.Name;
        _selectedItemDescription.text = selectedItem.Description;
        _selectedItemPrice.text = selectedItem.Price.ToString();

        _currentSelectedItemData = selectedItem;
    }
}