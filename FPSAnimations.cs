using UnityEngine;

public class FPSAnimations : MonoBehaviour
{
    private const string HandAttackTrigger = "HandAttack";

    [SerializeField] private Animator _animator;

    public void HandAttack()
    {
        _animator.SetTrigger(HandAttackTrigger);
    }
}
