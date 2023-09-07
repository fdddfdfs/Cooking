using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class State : MonoBehaviour
{
    protected Animator SelfAnimator;    

    [SerializeField] private Transition[] _transitions;

    private void Awake()
    {
        SelfAnimator = GetComponent<Animator>();
    }

    public void Enter()
    {
        this.enabled = true;
        TurnTransions(true);
    }

    public void Exit()
    {
        this.enabled = false;
        TurnTransions(false);
    }

    public IReadOnlyCollection<Transition> GetTransitions()
    {
        return _transitions;
    }

    private void TurnTransions(bool on)
    {
        foreach (var transition in _transitions)
        {
            transition.enabled = on;
        }
    }
}
