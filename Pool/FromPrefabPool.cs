using UnityEngine;

public sealed class FromPrefabPool : Pool<GameObject>
{
    private readonly GameObject _prefab;

    public FromPrefabPool(GameObject prefab, int startSize, bool shouldExpand = true)
        : base(shouldExpand)
    {
        _prefab = prefab;
        Init(startSize);
    }

    public override GameObject GetItem()
    {
        GameObject item = base.GetItem();
        item.SetActive(true);
        return item;
    }

    protected override GameObject CreateItem()
    {
        GameObject item = Object.Instantiate(_prefab);
        item.SetActive(false);
        return item;
    }

    protected override bool IsAvailable(GameObject pooledObject) => pooledObject.activeSelf == false;
}
