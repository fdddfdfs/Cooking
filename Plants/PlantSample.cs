using UnityEngine;

[CreateAssetMenu(fileName = "New PlantSample",menuName ="PlantSample/Create new",order = 51)]
public class PlantSample : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private float _growTime;

    public int GrowthStageAmount => _prefabs.Length;
    public string Name => _name;
    public float GrowTime => _growTime;

    public GameObject GetPrefab(int growthStage)
    {
        if (growthStage >= 0 && growthStage < _prefabs.Length)
            return _prefabs[growthStage];
        else
            throw new System.ArgumentOutOfRangeException();
    }
}
