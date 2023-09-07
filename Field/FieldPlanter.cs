using System;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class FieldPlanter : MonoBehaviour
{
    [SerializeField] private GameObject[] _fieldUnits;
    [SerializeField] private int _plantsInLine;
    [SerializeField] private float _plantOffsetY;
    [SerializeField] private GameObject _plantPlacePrefab;
    [SerializeField] private int _plantPlacesPoolSize;
    [SerializeField] private Material _plantPlaceHighlightMaterial;
    
    private IPlayer _player;
    private PlantFactory _plantFactory;

    private Pool<GameObject> _plantPlacesObjects;
    private List<PlantPlace> _plantPlaces;
    private List<Plant> _plants;

    [Inject]
    public void Init(PlantFactory plantFactory, IPlayer player)
    {
        _plantFactory = plantFactory;
        _player = player;
        _plantPlacesObjects = new FromPrefabPool(_plantPlacePrefab, _plantPlacesPoolSize);
        _plantPlaces = new List<PlantPlace>();
        _plants = new List<Plant>();
    }

    public Plant GetRandomPlant() => _plants[Random.Range(0, _plants.Count)];

    private void Start()
    {
        foreach (var fieldUnit in _fieldUnits)
        {
            CreatePlantPlaces(fieldUnit);
        }
        
        //Debug.Log(_player.PlayerItems == null);
    }

    private void CreatePlantPlaces(GameObject fieldUnit)
    {
        var renderer = fieldUnit.GetComponent<Renderer>();
        var plantPlaceSize = Mathf.Min(renderer.bounds.size.x,renderer.bounds.size.z)/ _plantsInLine;
        var currentPlaceToPlant = renderer.bounds.max - Vector3.up * _plantOffsetY - 
            Vector3.forward * plantPlaceSize/2 - 
            Vector3.right * plantPlaceSize /2;

        while(currentPlaceToPlant.z > renderer.bounds.min.z)
        {
            int counter = 0;

            while (currentPlaceToPlant.x - Vector3.right.x * plantPlaceSize * counter > renderer.bounds.min.x)
            {
                var plantPlace = _plantPlacesObjects.GetItem().AddComponent<PlantPlace>();
                plantPlace.Init(
                    currentPlaceToPlant - Vector3.right * plantPlaceSize * counter++,
                    _player,
                    _plantFactory,
                    _plantPlaceHighlightMaterial);
                _plantPlaces.Add(plantPlace);
                //var plant = _plantFactory.GetRandomPlant();
                //plantPlace.SetNewPlant(plant);
                //_plants.Add(plant);
            }

            currentPlaceToPlant -= Vector3.forward * plantPlaceSize;
        }
    }
}
