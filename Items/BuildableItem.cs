using UnityEngine;

public abstract class BuildableItem : Item, IUsable, IUpdatable, IShowable
{
    private readonly Building _building;
    private readonly Inventory _inventory;

    protected GameObject _buildableItem;
    
    private GameObject _prefab;

    protected abstract string BuildingResourceName { get; }

    protected BuildableItem(Building building, Inventory inventory, ItemData itemData) : base(itemData)
    {
        _building = building;
        _inventory = inventory;
    }

    public void Use()
    {
        if (_building.IsBuildable)
        {
            _building.SetBuilding();
            InstantiateNewBuilding();
            _building.UpdateBuilding(_buildableItem);
            _inventory.RemoveItem(_itemData, 1);
        }
    }

    public void Update()
    {
        _building.UpdateBuildableObject();
    }

    public void Show()
    {
        if (!_buildableItem)
        {
            InstantiateNewBuilding();
        }
        
        _building.UpdateBuilding(_buildableItem);
        _building.ChangeBuildingActive(true);
    }

    public void Hide()
    {
        _building.ChangeBuildingActive(false);
    }

    protected virtual void InstantiateNewBuilding()
    {
        if (!_prefab)
        {
            _prefab = ResourcesLoader.LoadObject(BuildingResourceName);
        }
        
        _buildableItem = Object.Instantiate(_prefab);
    }
}