using TMPro;
using UnityEngine;

public class InventoryView : MonoBehaviour
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

    public void Init(int inventoryGearSize, int inventoryMainSize)
    {
        _inventoryGearSize = inventoryGearSize;
        _inventoryMainSize = inventoryMainSize;
        
        GameObject inventoryCell = ResourcesLoader.LoadObject(InventoryCellResourceName);
        inventoryCell = Instantiate(inventoryCell);

        _gearCells = CreateCells(_inventoryGearSize, _inventoryMainSize, inventoryCell, _gear);
        _mainInventoryCells = CreateCells(_inventoryMainSize,0, inventoryCell, _mainInventory);
    }
    
    public void UpdateItem(int index, int newCount, InventoryItemData inventoryItemData)
    {
        Sprite itemSprite = inventoryItemData ? inventoryItemData.Sprite : null;
        
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
        
        UpdateCurrentItem(inventoryItemData);
    }

    private void UpdateCurrentItem(InventoryItemData inventoryItemData)
    {
        _currentItemName.text = inventoryItemData ? inventoryItemData.Name : string.Empty;
        _currentItemDescription.text = inventoryItemData ? inventoryItemData.Description : string.Empty;
    }

    private static InventoryCellView[] CreateCells(int size,int idOffset, GameObject prefab, Transform parent)
    {
        var cells = new InventoryCellView[size];
        for (var i = 0; i < size; i++)
        {
            cells[i] = ResourcesLoader.InstantiateLoadedComponent<InventoryCellView>(prefab);
            cells[i].Init(i + idOffset);
            cells[i].transform.parent = parent;
        }

        return cells;
    }
}