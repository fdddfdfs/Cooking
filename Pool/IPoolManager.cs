public interface IPoolManager<T,T1> 
{
    T1 TakeItem(T key);
}

public interface IPlantObjects : IPoolManager<Plant,UnityEngine.GameObject>
{
}
