using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlantData : IPlantData
{
    private List<PlantSample> _plantSamples;

    public PlantData(PlantSample[] plantSamples)
    {
        _plantSamples = plantSamples
            .ToList();
    }

    public IReadOnlyList<PlantSample> GetPlantSamples() => _plantSamples;

    public PlantSample GetRandomPlantSample() => _plantSamples[Random.Range(0, _plantSamples.Count)];

    public PlantSample GetPlantSampleByName(string name) => _plantSamples.Find((sample) => sample.Name == name);
}
