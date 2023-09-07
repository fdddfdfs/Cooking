using System.Collections;
using UnityEngine;

public class AttackState : State
{
    [SerializeField] private float _attackCooldown;

    private IMoleTargetter _moleTargetter;
    private ITargettable _currentTarget;

    private void OnEnable()
    {
        if (_moleTargetter == null)
            _moleTargetter = GetComponent<IMoleTargetter>();

        _currentTarget = _moleTargetter.GetCurrentTarget();
        Attack();

        SelfAnimator.SetTrigger(Animations.Attack);
    }

    private void Attack()
    {
        _currentTarget.TakeDamage(25);
        StartCoroutine(WaitForCooldown(_attackCooldown));
    }

    private IEnumerator WaitForCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        Attack();
    }
}
