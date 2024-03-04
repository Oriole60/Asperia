using GameDevTV.Inventories;
using RPG.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseSlotState : PlayerBaseState
{
    int indexSlotUse;
    GameObject user;

    private const float CrossFadeDuration = 0.1f;


    public PlayerUseSlotState(PlayerStateMachine stateMachine,int index, GameObject user) : base(stateMachine) 
    {
        indexSlotUse = index;
        this.user = user;
    }

    public override void Enter()
    {

        ActionItem actionItem = stateMachine.ActionStore.GetAction(indexSlotUse);
        if(actionItem is Ability ability)
        {
            Debug.Log("Using Slot");
            stateMachine.Animator.CrossFadeInFixedTime(ability.StateName, CrossFadeDuration);
        }

        stateMachine.ActionStore.UseActionSlot(indexSlotUse, user);


    }

    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, GameConstant.USESLOT_TAG_ANIMATIONSTATE);
        Debug.Log(normalizedTime);
        if(normalizedTime >= 1f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {

    }
}
