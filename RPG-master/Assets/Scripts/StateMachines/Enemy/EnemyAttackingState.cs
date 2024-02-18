using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack");

    private const float TransitionDuration = 0.1f;

    private bool alreadyAppliedForce;
    private Attack attack;
    private float calculatedNextComboChange = 0f;

    public EnemyAttackingState(EnemyStateMachine stateMachine,int attackIndex) : base(stateMachine) 
    {
        attack = stateMachine.Fighter.GetCurrentWeaponConfig().GetAttacksData()[attackIndex];
    }

    public override void Enter()
    {
        WeaponDamage weapon = stateMachine.Fighter.GetWeaponHandler().GetWeaponDamage();

        weapon.SetAttack(stateMachine.BaseStats.GetStat(Stat.Damage), attack.Knockback);

        //stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);

        calculatedNextComboChange = Random.value;

        FacePlayer();

        stateMachine.ResetAttackIdleCooldown();
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");
        if (normalizedTime < 1f)
        {
            if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }


        }
        else if (normalizedTime >= 1)
        {
            if (calculatedNextComboChange <= stateMachine.ComboChance)
            {
                TryComboAttack(normalizedTime);
            }
            else
            {
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            }
        }
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) { return; }
        Debug.Log("Apply Force on Enemy");
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

        alreadyAppliedForce = true;
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (attack.CombonNextStateIndex == -1) 
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return; 
        }

        if (normalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState
        (
            new EnemyAttackingState
            (
                stateMachine,
                attack.CombonNextStateIndex
            )
        );
    }

    public override void Exit() { }
}
