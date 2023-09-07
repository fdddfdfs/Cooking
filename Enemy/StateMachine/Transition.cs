using UnityEngine;
using UnityEngine.Events;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private State _targetState;

    //public State TargetState => _targetState;
    public event UnityAction<State> OnReady;

    protected void Transit()
    {
        if (this.enabled)
            OnReady?.Invoke(_targetState);
    }
}
