using System.Collections.Generic;
using UnityEngine;

public class PlantGameObjects : IPlantObjects
{
    private Dictionary<string ,List<Pool<GameObject>>> _plantGameObjects;
    private readonly int _poolsSize = 2;
    private IReadOnlyList<PlantSample> _plantSamples;

    public PlantGameObjects(IPlantData plantData)
    {
        _plantSamples = plantData.GetPlantSamples();
        Init(_plantSamples);   
    }

    public GameObject TakeItem(Plant plant)
    {
        if (_plantGameObjects.ContainsKey(plant.Name))
        {
            return _plantGameObjects[plant.Name][plant.CurrentGrowthStage].GetItem();
        }
        else
        {
            Debug.LogError($"no object for plant named: {plant.Name}");
            return null;
        }
    }

    private void Init(IReadOnlyList<PlantSample> plantSamples)
    {
        _plantGameObjects = new Dictionary<string, List<Pool<GameObject>>>();

        foreach (PlantSample plantSample in plantSamples)
        {
            CreatePools(plantSample);
        }
    }

    private void CreatePools(PlantSample plantSample)
    {
        _plantGameObjects.Add(plantSample.Name, new List<Pool<GameObject>>());

        for (var i = 0; i < plantSample.GrowthStageAmount; i++)
        {
            GameObject plantPrefab = plantSample.GetPrefab(i);

            if (i == plantSample.GrowthStageAmount - 1)
            {
                if (!plantPrefab.TryGetComponent<LastStagePlant>(out _))
                {
                    plantPrefab.AddComponent<LastStagePlant>();
                }
            }
            
            _plantGameObjects[plantSample.Name].Add(new FromPrefabPool(plantPrefab, _poolsSize));
        }
    }
}
