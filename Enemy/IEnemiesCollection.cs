using System.Collections.Generic;
using UnityEngine;

public interface IEnemiesCollection
{
    public IEnumerable<GameObject> GetEnemies();
}