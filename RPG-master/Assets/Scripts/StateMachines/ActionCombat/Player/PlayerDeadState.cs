using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Dead");

    private const float CrossFadeDuration = 0.1f;
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
        stateMachine.Fighter.GetWeaponHandler().GetWeaponDamage().gameObject.SetActive(false);
    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
