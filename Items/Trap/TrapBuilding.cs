using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Animator))]
public class TrapBuilding : MonoBehaviour
{
    private const string ActivateTrigger = "Activate";
    
    private readonly int _activateTrigger = Animator.StringToHash(ActivateTrigger);

    private LayerMask _triggerMask;
    private BoxCollider _collider;
    private Animator _animator;
    
    public void Init()
    {
        _collider.enabled = true;
        _collider.isTrigger = true;
        _triggerMask = LayerMask.GetMask("Mole");
    }
    
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer & _triggerMask) != 0)
        {
            _animator.SetTrigger(_activateTrigger);

            if (other.TryGetComponent(out ITrappable trappable))
            {
                trappable.ActivateTrap();
            }
            else
            {
                Debug.LogWarning($"{other.name} doesnt have ITrappable component");
            }
        }
}
}