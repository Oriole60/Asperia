using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Dead");

    private const float CrossFadeDuration = 0.1f;

    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
        stateMachine.Fighter.GetWeaponHandler().GetWeaponDamage().gameObject.SetActive(false);
        GameObject.Destroy(stateMachine.Target);
    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
