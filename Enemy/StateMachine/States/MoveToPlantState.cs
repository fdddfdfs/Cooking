using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(IMoleTargetter))]
public class MoveToPlantState : State
{
    private IMoleTargetter _moleTargetter;
    private NavMeshAgent _agent;

    private void OnEnable()
    {
        Init();
        _moleTargetter.FindNewTarget();
        _agent.isStopped = false;
        _agent.SetDestination(_moleTargetter.GetCurrentTarget().Position);
        SelfAnimator.SetTrigger(Animations.Run);
    }

    private void Init()
    {
        if (_moleTargetter == null)
        {
            _moleTargetter = GetComponent<IMoleTargetter>();
            _agent = _moleTargetter.GetMoleAgent();
        }
    }

    private void OnDisable()
    {
        _agent.isStopped = true;
    }
}
