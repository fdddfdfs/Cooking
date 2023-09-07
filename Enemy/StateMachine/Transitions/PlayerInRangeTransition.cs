using UnityEngine;

public class PlayerInRangeTransition : Transition
{
    [SerializeField] private LayerMask _layerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (_layerMask == (1 << other.gameObject.layer))
            Transit();
    }
}
