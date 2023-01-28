using UnityEngine;

public class Trap : IUsable, IUpdatable, IShowable
{
    private const string TrapResourceName = "Trap";
    
    private readonly Building _building;
    private GameObject _trap;
    
    public Trap(Building building)
    {
        _building = building;
        _trap = ResourcesLoader.LoadObject(TrapResourceName);
        _trap = Object.Instantiate(_trap);
    }

    public void Use()
    {
        if (_building.SetBuilding())
        {
            _trap = Object.Instantiate(_trap);
            _building.UpdateBuilding(_trap);
        }
    }

    public void Update()
    {
        _building.UpdateBuildableObject();
    }

    public void Show()
    {
        _building.UpdateBuilding(_trap);
        _building.ChangeBuildingActive(true);
    }

    public void Hide()
    {
        _building.ChangeBuildingActive(false);
    }
}