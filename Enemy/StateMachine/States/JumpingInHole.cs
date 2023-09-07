using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(IMoleTargetter))]
public class JumpingInHole : State
{
    [SerializeField] private float _rotationDuration;
    [SerializeField] private float _movingIntoHoleDuration;
    [SerializeField] private float _jumpPower;

    private IMoleTargetter _moleTargetter;
    private Vector3 _holePosition;

    private void OnEnable()
    {
        if (_moleTargetter == null)
            _moleTargetter = GetComponent<IMoleTargetter>();

        _holePosition = _moleTargetter.GetHolePosition();
        Animate();
    }

    private void Animate()
    {
        SelfAnimator.SetTrigger(Animations.HoleJump);
        this.transform.DOLookAt(_holePosition, _rotationDuration);
        this.transform.DOJump(this.transform.position, _jumpPower, 1, _movingIntoHoleDuration);
        this.transform.DOMoveX(_holePosition.x, _movingIntoHoleDuration);
        this.transform.DOMoveZ(_holePosition.z, _movingIntoHoleDuration);
    }
}
