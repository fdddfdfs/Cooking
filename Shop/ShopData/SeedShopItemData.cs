using UnityEngine;

[CreateAssetMenu(fileName = "SeedShopItem", menuName = "Data/SeedShopItem")]
public class SeedShopItemData : ShopItemData
{
    [SerializeField] private PlantSample _plantSample;
    [SerializeField] private ItemData _plantFetus;

    public PlantSample PlantSample => _plantSample;

    public ItemData PlantFetus => _plantFetus;
}