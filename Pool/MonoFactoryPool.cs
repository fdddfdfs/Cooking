using UnityEngine;

public class MonoFactoryPool<TItem, TFabric, TFabricKey> : Pool<TItem>
    where TItem : MonoBehaviour
    where TFabric : Factory<TItem, TFabricKey>
{
    private TFabric _fabric;
    private TFabricKey _factoryKey;

    public MonoFactoryPool(bool shouldExpand, TFabric fabric, TFabricKey defaultFactoryKey) : base(shouldExpand)
    {
        _fabric = fabric;
        _factoryKey = defaultFactoryKey;
    }

    public void ChangeKey(TFabricKey newKey)
    {
        _factoryKey = newKey;
    }

    protected override bool IsAvailable(TItem pooledObject)
    {
        return pooledObject.gameObject.activeSelf;
    }

    protected override TItem CreateItem()
    {
        return _fabric.GetItem(_factoryKey);
    }
}