using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    //private float previousFrameTime; Remove this because this is redundant 
    private bool alreadyAppliedForce;

    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Fighter.GetCurrentWeaponConfig().GetAttacksData()[attackIndex];
    }

    public override void Enter()
    {
        WeaponDamage weapon = stateMachine.Fighter.GetWeaponHandler().GetWeaponDamage();
        weapon.SetAttack(stateMachine.BaseStats.GetStat(Stat.Damage), attack.Knockback);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        FaceTarget();

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        if (normalizedTime < 1f)
        {
            if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
    }

    public override void Exit()
    {

    }

    private void TryComboAttack(float normalizedTime)
    {
        if (attack.CombonNextStateIndex == -1) { return; }

        if (normalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState
        (
            new PlayerAttackingState
            (
                stateMachine,
                attack.CombonNextStateIndex
            )
        );
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) { return; }

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

        alreadyAppliedForce = true;
    }
}
