using UnityEngine;
using System.Collections;

public class PlayerOutOfRangeTransition : Transition
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _timeOffset;

    private Coroutine _waiting;

    private void OnTriggerEnter(Collider other)
    {
        if (_layerMask == (1 << other.gameObject.layer) && _waiting != null)
        {
            StopCoroutine(_waiting);
            _waiting = null;
        }
    }

    private void OnDisable()
    {
        if (_waiting != null)
        {
            StopCoroutine(_waiting);
            _waiting = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_layerMask == (1 << other.gameObject.layer))
            _waiting = StartCoroutine(WaitTimeOffset());
    }

    private IEnumerator WaitTimeOffset()
    {
        yield return new WaitForSeconds(_timeOffset);
        Transit();
    }
}
