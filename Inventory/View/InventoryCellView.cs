using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCellView : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TMP_Text _count;
    [SerializeField] private Sprite _emptyItemSprite;

    private readonly string _zeroCount = string.Empty;
    private bool _isEmpty;
    
    public int Index { get; private set; }

    public Sprite Sprite => _itemImage.sprite;

    public Image Image => _itemImage;

    public void Init(int index)
    {
        Index = index;
        _isEmpty = true;
        _count.text = _zeroCount;
    }

    public void ChangeCellItemSprite(Sprite newSprite)
    {
        if (!newSprite)
        {
            _itemImage.sprite = _emptyItemSprite;
            _isEmpty = true;
        }

        _isEmpty = false;
        _itemImage.sprite = newSprite;
    }

    public void ChangeCellItemCount(int newCount)
    {
        _count.text = newCount != 0 ? newCount.ToString() : _zeroCount;
    }
}