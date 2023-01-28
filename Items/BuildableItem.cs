using UnityEngine;

public abstract class BuildableItem : Item, IUsable, IUpdatable, IShowable
{
    private readonly Building _building;
    private readonly Inventory _inventory;
    
    private GameObject _buildableItem;
    
    protected abstract string BuildingResourceName { get; }

    protected BuildableItem(Building building, Inventory inventory, InventoryItemData itemData) : base(itemData)
    {
        _building = building;
        _inventory = inventory;
        _buildableItem = ResourcesLoader.LoadObject(BuildingResourceName);
        _buildableItem = Object.Instantiate(_buildableItem);
    }

    public void Use()
    {
        if (_building.SetBuilding())
        {
            _buildableItem = Object.Instantiate(_buildableItem);
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
}