using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTester : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _redMaterial;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private ItemData _turret;
    [SerializeField] private ItemData _trap;
    [SerializeField] private ItemData _turretAmmoData;
    [SerializeField] private GameObject _enemiesCollection;

    private Building _building;
    private Inventory _inventory;
    private PlayerItems _playerItems;
    private PlayerRaycaster _playerRaycaster;

    private void Awake()
    {
        _playerRaycaster = new PlayerRaycaster(_playerCamera);
        _building = new Building(_playerCamera, _greenMaterial, _redMaterial);
        _inventory = new Inventory(_canvas);
        _playerItems = new PlayerItems(
            _building,
            _inventory,
            _trap,
            _turret,
            _enemiesCollection.GetComponent<IEnemiesCollection>(),
            _canvas,
            _turretAmmoData);

        _inventory.AddItem(_turret, 2);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            _inventory.SwapActive();
        }
        
        _playerItems.Update();
        _playerRaycaster.Update();
    }
}