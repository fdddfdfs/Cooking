using UnityEngine;

public class PlantFactory : Factory<Plant, SeedShopItemData>
{
    private IPlantObjects _plantObjects;
    private IPlantData _plantData;

    public PlantFactory(IPlantData plantData, IPlantObjects plantObjects)
    {
        _plantData = plantData;
        _plantObjects = plantObjects;
    }

    public Plant GetItem(SeedShopItemData plantSample)
    {
        return CreatePlant(plantSample);
    }

    public Plant GetClearPlant()
    {
        return new Plant(_plantObjects);
    }

    private Plant CreatePlant(SeedShopItemData seedShopItemData)
    {
        return new Plant(
            seedShopItemData, 
            0, 
            Random.Range(seedShopItemData.PlantSample.GrowTime / 2, seedShopItemData.PlantSample.GrowTime),
            _plantObjects);
    }
}
