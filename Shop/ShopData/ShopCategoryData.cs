using UnityEngine;

[CreateAssetMenu(fileName = "ShopCategoryItem", menuName = "Data/ShopCategoryItem")]
public class ShopCategoryData : ScriptableObject
{
    [SerializeField] private ShopCategoryType _type;
    [SerializeField] private Sprite _sprite;

    public ShopCategoryType Type => _type;

    public Sprite Sprite => _sprite;
}