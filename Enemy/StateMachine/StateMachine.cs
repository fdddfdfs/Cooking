using UnityEngine;

[RequireComponent(typeof(Transition))]
public class StateMachine : MonoBehaviour
{
    [SerializeField] private State _startState;
    [SerializeField] private Transition[] _allStateTransitions;

    private State _currentState;

    private void OnEnable()
    {
        TurnOnAllStateTransitions();
        Transit(_startState);
    }

    private void OnDisable()
    {
        TurnOffAllStateTransitions();
    }

    private void TurnOnAllStateTransitions()
    {
        foreach (var transition in _allStateTransitions)
        {
            transition.enabled = true;
            transition.OnReady += Transit;
        }
    }

    private void TurnOffAllStateTransitions()
    {
        foreach (var transition in _allStateTransitions)
        {
            transition.enabled = false;
            transition.OnReady -= Transit;
        }
    }

    private void Transit(State toState)
    {
        if (_currentState!=null)
        {
            _currentState.Exit();
            UnlistenTransitions(_currentState);
        }

        toState.Enter();
        ListenToTransitions(toState);
        _currentState = toState;
    }

    private void ListenToTransitions(State state)
    {
        var transitions = state.GetTransitions();

        foreach (var transition in transitions)
        {
            transition.OnReady += Transit;
        }
    }

    private void UnlistenTransitions(State state)
    {
        var transitions = state.GetTransitions();

        foreach (var transition in transitions)
        {
            transition.OnReady -= Transit;
        }
    }
}
