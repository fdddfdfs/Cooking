using System.Collections.Generic;

public interface IPlantData 
{
    IReadOnlyList<PlantSample> GetPlantSamples();
    
    PlantSample GetRandomPlantSample();

    public PlantSample GetPlantSampleByName(string name);
}
