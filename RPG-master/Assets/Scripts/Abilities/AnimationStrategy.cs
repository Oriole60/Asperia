using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "Animation", menuName = "Abilities/AnimationStrategy/Animation", order = 0)]
public class AnimationStrategy : ScriptableObject
{
    [SerializeField] AnimationClip replacementClip;

    public void SetUpAnimation(AbilityData data, Action finished)
    {
        Fighter fighter = data.GetUser().GetComponent<Fighter>();

        fighter.ClipOverrides[GameConstant.USESLOT_TAG_ANIMATIONSTATE] = replacementClip;
        fighter.AnimatorOverrideController.ApplyOverrides(fighter.ClipOverrides);

        finished();
    } 


}


