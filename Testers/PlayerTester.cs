using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTester : MonoBehaviour, IPlayer
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _redMaterial;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private ItemData _turret;
    [SerializeField] private ItemData _trap;
    [SerializeField] private ItemData _turretAmmoData;
    [SerializeField] private GameObject _enemiesCollection;
    [SerializeField] private List<ShopItemData> _shopItemData;
    [SerializeField] private List<ShopCategoryData> _shopCategoryData;
    [SerializeField] private PlayerMessageView _playerMessageView;

    private Building _building;
    private PlayerRaycaster _playerRaycaster;

    private MenuOrderer _menuOrderer;
    
    public Inventory Inventory { get; private set; }

    public PlayerStats PlayerStats { get; private set; }

    public PlayerMessageView PlayerMessageView => _playerMessageView;

    public PlayerItems PlayerItems { get; private set; }

    public Shop Shop { get; private set; }

    private void Awake()
    {
        _playerRaycaster = new PlayerRaycaster(_playerCamera);
        _building = new Building(_playerCamera, _greenMaterial, _redMaterial);
        
        _menuOrderer = new MenuOrderer(_canvas, _shopItemData, _shopCategoryData, this);

        Inventory = _menuOrderer[typeof(Inventory)] as Inventory;
        if (Inventory == null) throw new Exception("Inventory must be created");

        Inventory.ChangeMenuActive(false);
        Inventory.AddItem(_turret, 2);
        Inventory.AddItem(_turretAmmoData, 10);
        
        PlayerItems = new PlayerItems(
            _menuOrderer,
            _building,
            Inventory,
            _trap,
            _turret,
            _enemiesCollection.GetComponent<IEnemiesCollection>(),
            _canvas,
            _turretAmmoData);
        PlayerStats = new PlayerStats(1000000);

        Shop = _menuOrderer[typeof(Shop)] as Shop;
        if (Shop == null) throw new Exception("Shop must be created");
        
        Shop.ChangeMenuActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            _menuOrderer.SwapMenuActive(typeof(Inventory));
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            _menuOrderer.SwapMenuActive(typeof(Shop));
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _menuOrderer.CloseUpperMenu();
        }
        
        PlayerItems.Update();
        _playerRaycaster.Update();
    }
}