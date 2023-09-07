using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private const string InventoryCellResourceName = "InventoryCell";

    [SerializeField] private Transform _mainInventory;
    [SerializeField] private Transform _gear;
    [SerializeField] private TMP_Text _selectedItemName;
    [SerializeField] private TMP_Text _selectedItemDescription;

    private InventoryCellView[] _gearCells;
    private InventoryCellView[] _mainInventoryCells;
    private int _inventoryGearSize;
    private int _inventoryMainSize;
    private Inventory _inventory;

    private int _dragCellId;
    private Image _dragImage;

    public void Init(Inventory inventory, int inventoryGearSize, int inventoryMainSize)
    {
        _inventoryGearSize = inventoryGearSize;
        _inventoryMainSize = inventoryMainSize;
        _inventory = inventory;
        
        GameObject inventoryCell = ResourcesLoader.LoadObject(InventoryCellResourceName);
        inventoryCell = Instantiate(inventoryCell);

        _gearCells = CreateCells(_inventoryGearSize, _inventoryMainSize, inventoryCell, _gear);
        _mainInventoryCells = CreateCells(_inventoryMainSize,0, inventoryCell, _mainInventory);

        var dragger = ResourcesLoader.InstantiateLoadedComponent<InventoryCellView>(inventoryCell);
        dragger.transform.SetParent(_mainInventory.parent, false);
        _dragImage = dragger.Image;
        _dragImage.gameObject.SetActive(false);
        _dragImage.raycastTarget = false;
        Destroy(dragger);
    }

    public void UpdateItem(int index, int newCount, ItemData itemData)
    {
        Sprite itemSprite = itemData ? itemData.Sprite : null;

        InventoryCellView inventoryCellView = GetInventoryView(index);
        
        inventoryCellView.ChangeCellItemCount(newCount);
        inventoryCellView.ChangeCellItemSprite(itemSprite);
    }
    
    public void UpdateSelectedItem(ItemData itemData)
    {
        _selectedItemName.text = itemData ? itemData.Name : string.Empty;
        _selectedItemDescription.text = itemData ? itemData.Description : string.Empty;
    }

    private InventoryCellView GetInventoryView(int index)
    {
        return index < _inventoryMainSize ? _mainInventoryCells[index] : _gearCells[index - _inventoryMainSize];
    }

    private static InventoryCellView[] CreateCells(int size,int idOffset, GameObject prefab, Transform parent)
    {
        var cells = new InventoryCellView[size];
        for (var i = 0; i < size; i++)
        {
            cells[i] = ResourcesLoader.InstantiateLoadedComponent<InventoryCellView>(prefab);
            cells[i].Init(i + idOffset);
            cells[i].transform.SetParent(parent, false);
        }

        return cells;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null) return;

        var inventoryCellView = eventData.pointerEnter.GetComponentInParent<InventoryCellView>();
        if (inventoryCellView)
        {
            _dragCellId = inventoryCellView.Index;
            _dragImage.gameObject.SetActive(true);
            _dragImage.sprite = inventoryCellView.Sprite;
            inventoryCellView.ChangeCellItemSprite(null);
        }
        else
        {
            _dragCellId = -1;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null || _dragCellId == -1) return;
        
        GetInventoryView(_dragCellId).ChangeCellItemSprite(_dragImage.sprite);
        _dragImage.gameObject.SetActive(false);

        var inventoryCellView = eventData.pointerEnter.GetComponentInParent<InventoryCellView>();
        if (inventoryCellView)
        {
            _inventory.SwapItems(_dragCellId, inventoryCellView.Index);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _dragImage.transform.position = Mouse.current.position.ReadValue();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var inventoryCellView = eventData.pointerEnter.GetComponentInParent<InventoryCellView>();
        if (inventoryCellView)
        {
            _inventory.ShowSelectedItem(inventoryCellView.Index);
        }
    }
}