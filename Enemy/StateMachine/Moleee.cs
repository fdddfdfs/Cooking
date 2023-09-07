using UnityEngine;
using Zenject;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Moleee : MonoBehaviour, IMoleTargetter
{
    [SerializeField] private GameObject _hole;
    [SerializeField] private NavMeshAgent _agent;

    [Inject] private FieldPlanter _fieldPlanter;

    private ITargettable _currentTarget;

    private void OnEnable()
    {
        UpdateHolePosition();
    }

    public Vector3 GetHolePosition() => _hole.transform.position;

    public ITargettable GetCurrentTarget()
    {
        if (_currentTarget == null)
            FindNewTarget();

        return _currentTarget;
    }

    private void UpdateHolePosition()
    {
        _hole.transform.position = 
            new Vector3(this.transform.position.x, _hole.transform.position.y, this.transform.position.z); ;
    }

    public NavMeshAgent GetMoleAgent() => _agent;

    public void FindNewTarget()
    {
        _currentTarget = _fieldPlanter.GetRandomPlant();
    }
}
