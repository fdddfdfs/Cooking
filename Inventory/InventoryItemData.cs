using UnityEngine;

public class InventoryItemData : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _maxStack;

    public Sprite Sprite => _sprite;

    public string Name => _name;

    public string Description => _description;

    public int MaxStack => _maxStack;
}