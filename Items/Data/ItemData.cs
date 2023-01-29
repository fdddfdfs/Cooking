using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _maxStack;
    [SerializeField] private ItemType _itemType;

    public Sprite Sprite => _sprite;

    public string Name => _name;

    public string Description => _description;

    public int MaxStack => _maxStack;

    public ItemType ItemType => _itemType;
}