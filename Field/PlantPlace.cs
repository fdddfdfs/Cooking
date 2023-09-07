using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PlantPlace : MonoBehaviour , IRaycastable
{
    private Vector3 _position;
    private PlayerItems _playerItems;
    private PlayerMessageView _playerMessageView;
    private Inventory _inventory;
    private PlantFactory _plantFactory;

    private MeshRenderer _mesh;
    private Material _highlightMaterial;
    private Material _defaultMaterial;

    public Plant Plant { get; private set; }

    public void Init(Vector3 position, IPlayer player, PlantFactory plantFactory, Material highlightMaterial)
    {
        gameObject.SetActive(true);
        _position = position;
        _playerItems = player.PlayerItems;
        _playerMessageView = player.PlayerMessageView;
        _inventory = player.Inventory;
        _plantFactory = plantFactory;

        transform.position = position;

        _highlightMaterial = highlightMaterial;
        _mesh = GetComponent<MeshRenderer>();
        _defaultMaterial = _mesh.material;

        Plant = _plantFactory.GetClearPlant();
    }

    public void Hit()
    {
        if (Plant.CurrentPlant && Plant.IsFullGrown)
        {
            _playerMessageView.SetText("Hit E pickup plant");
            Plant.HighlightLastStagePlant(_highlightMaterial);
            return;
        }

        if (_playerItems.CurrentItemData && _playerItems.CurrentItemData.ItemType == ItemType.Seed)
        {
            _playerMessageView.SetText("Hit E to plant seed");
            _mesh.material = _highlightMaterial;
        }
    }

    public void UnHit()
    {
        _playerMessageView.ClearText();

        if (Plant.CurrentPlant && Plant.IsFullGrown)
        {
            Plant.DeHighlightLastStagePlant();
        }
        else
        {
            _mesh.material = _defaultMaterial;
        }
    }

    public void Use()
    {
        if (Plant.Attached)
        {
            _inventory.AddItem(Plant.CurrentPlant.PlantFetus, 1);
            Plant.ClearPlant();
            return;
        }
        
        if (_playerItems.CurrentItemData is SeedShopItemData seedShopItemData)
        {
            bool result = _inventory.RemoveItem(_playerItems.CurrentItemData, 1);

            if (result)
            {
                SetNewPlant(seedShopItemData);
            }
        }
    }
    
    private void SetNewPlant(SeedShopItemData seedShopItemData)
    {
        _mesh.enabled = false;
        Plant.SetNewPlant(
            seedShopItemData, 
            0, 
            Random.Range(seedShopItemData.PlantSample.GrowTime / 2, seedShopItemData.PlantSample.GrowTime),
            _position);
    }
}
