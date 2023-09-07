using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOutState : State
{
    [SerializeField] private float _jumpHeight;
    [SerializeField] private GameObject _hole;
    private void OnEnable()
    {
        SelfAnimator.SetTrigger(Animations.HoleJumpOut);
    }
}
