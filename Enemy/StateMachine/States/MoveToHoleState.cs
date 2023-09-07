using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(IMoleTargetter))]
public class MoveToHoleState : State
{
    private IMoleTargetter _moleTargetter;
    private NavMeshAgent _agent;

    private void OnEnable()
    {
        Init();
        _agent.isStopped = false;
        _agent.SetDestination(_moleTargetter.GetHolePosition());
        SelfAnimator.SetTrigger(Animations.Run);
    }

    private void OnDisable()
    {
        _agent.isStopped = true;
    }

    private void Init()
    {
        if (_moleTargetter == null)
        {
            _moleTargetter = GetComponent<IMoleTargetter>();
            _agent = _moleTargetter.GetMoleAgent();
        }
    }
}
