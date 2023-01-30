using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private const string InventoryCellResourceName = "InventoryCell";

    [SerializeField] private Transform _mainInventory;
    [SerializeField] private Transform _gear;
    [SerializeField] private TMP_Text _currentItemName;
    [SerializeField] private TMP_Text _currentItemDescription;

    private InventoryCellView[] _gearCells;
    private InventoryCellView[] _mainInventoryCells;
    private int _inventoryGearSize;
    private int _inventoryMainSize;
    private Inventory _inventory;

    private int _dragCellId;

    public void Init(Inventory inventory, int inventoryGearSize, int inventoryMainSize)
    {
        _inventoryGearSize = inventoryGearSize;
        _inventoryMainSize = inventoryMainSize;
        _inventory = inventory;
        
        GameObject inventoryCell = ResourcesLoader.LoadObject(InventoryCellResourceName);
        inventoryCell = Instantiate(inventoryCell);

        _gearCells = CreateCells(_inventoryGearSize, _inventoryMainSize, inventoryCell, _gear);
        _mainInventoryCells = CreateCells(_inventoryMainSize,0, inventoryCell, _mainInventory);
    }

    public void ChangeActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SwapActive()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }
    
    public void UpdateItem(int index, int newCount, ItemData itemData)
    {
        Sprite itemSprite = itemData ? itemData.Sprite : null;
        
        if (index > _inventoryMainSize)
        {
            int realIndex = index - _inventoryMainSize;
            _gearCells[realIndex].ChangeCellItemSprite(itemSprite);
            _gearCells[realIndex].ChangeCellItemCount(newCount);
        }
        else
        {
            _mainInventoryCells[index].ChangeCellItemSprite(itemSprite);
            _mainInventoryCells[index].ChangeCellItemCount(newCount);
        }
        
        UpdateCurrentItem(itemData);
    }

    private void UpdateCurrentItem(ItemData itemData)
    {
        _currentItemName.text = itemData ? itemData.Name : string.Empty;
        _currentItemDescription.text = itemData ? itemData.Description : string.Empty;
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
            _dragCellId = inventoryCellView.ID;
        }
        else
        {
            _dragCellId = -1;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null || _dragCellId == -1) return;

        var inventoryCellView = eventData.pointerEnter.GetComponentInParent<InventoryCellView>();
        if (inventoryCellView)
        {
            _inventory.SwapItems(_dragCellId, inventoryCellView.ID);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}