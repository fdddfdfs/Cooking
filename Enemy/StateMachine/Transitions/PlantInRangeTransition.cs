using UnityEngine;

[RequireComponent(typeof(IMoleTargetter))]
public class PlantInRangeTransition : Transition
{
    [SerializeField] private float _attackRange;

    private IMoleTargetter _moleTargetter;
    private ITargettable _currentTarget;

    private void OnEnable()
    {
        if (_moleTargetter == null)
            _moleTargetter = GetComponent<IMoleTargetter>();

        _currentTarget = _moleTargetter.GetCurrentTarget();
    }

    private void Update()
    {
        if ((this.transform.position - _currentTarget.Position).sqrMagnitude <= _attackRange * _attackRange)
            Transit();
    }
}
