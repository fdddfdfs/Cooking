using System.Collections.Generic;
using UnityEngine;

public class TestEnemies : MonoBehaviour, IEnemiesCollection
{
    [SerializeField] private List<GameObject> _enemies;


    public IEnumerable<GameObject> GetEnemies()
    {
        return _enemies;
    }
}