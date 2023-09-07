using UnityEngine;

public class HoleInRangeTransition : Transition
{
    [SerializeField] private float _range;

    private IMoleTargetter _moleTargetter;
    private Vector3 _currentHolePosition;

    private void OnEnable()
    {
        if (_moleTargetter == null)
            _moleTargetter = GetComponent<IMoleTargetter>();

        _currentHolePosition = _moleTargetter.GetHolePosition();
    }

    private void Update()
    {
        if (_range * _range >= (this.transform.position - _currentHolePosition).sqrMagnitude)
            Transit();
    }
}
