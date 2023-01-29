using UnityEngine;

public abstract class BuildableItem : Item, IUsable, IUpdatable, IShowable
{
    private readonly Building _building;
    private readonly Inventory _inventory;
    
    protected GameObject _buildableItem;
    
    protected abstract string BuildingResourceName { get; }

    protected BuildableItem(Building building, Inventory inventory, ItemData itemData) : base(itemData)
    {
        _building = building;
        _inventory = inventory;
        _buildableItem = ResourcesLoader.LoadObject(BuildingResourceName);
        InstantiateNewBuilding();
    }

    public void Use()
    {
        if (_building.SetBuilding())
        {
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
        _building.UpdateBuilding(_buildableItem);
        _building.ChangeBuildingActive(true);
    }

    public void Hide()
    {
        _building.ChangeBuildingActive(false);
    }

    protected virtual void InstantiateNewBuilding()
    {
        _buildableItem = Object.Instantiate(_buildableItem);
    }
}