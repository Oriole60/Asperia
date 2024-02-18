using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackIdle : EnemyBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("AttackIdle");

    private const float CrossFadeDuration = 0.1f;

    private const float MinRandomAttackIdle = 0.5f;
    private const float MaxRandomAttackIdle = 1.2f;

    private float outOfCombatRange;

    public EnemyAttackIdle(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
        stateMachine.AttackIdleCount = stateMachine.AttackIdleCooldown * Random.Range(MinRandomAttackIdle, MaxRandomAttackIdle);

    }

    public override void Tick(float deltaTime)
    {
        stateMachine.AttackIdleCount -= deltaTime;

        if (!IsInCombatRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }
        if (stateMachine.AttackIdleCount <= 0)
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine, 0));
            return;
        }
    }

    public override void Exit()
    {
        
    }

    private bool IsInCombatRange()
    {
        if (stateMachine.Player.IsDead()) { return false; }

        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        if (!stateMachine.Fighter.GetCurrentWeaponConfig().HasProjectile())
        {
            outOfCombatRange = stateMachine.Fighter.GetCurrentWeaponConfig().GetRange() * stateMachine.Fighter.GetCurrentWeaponConfig().GetRange();
            return playerDistanceSqr <= outOfCombatRange * outOfCombatRange;
        }
        else
        {
            float attackRange = stateMachine.Fighter.GetCurrentWeaponConfig().GetRange();
            return playerDistanceSqr <= attackRange * attackRange;
        }
         
    }

}
