using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animation", menuName = "Abilities/AnimationStrategy/Animation", order = 0)]
public class AnimationStrategy : ScriptableObject
{
    [SerializeField] AnimationClip replacementClip;
    [SerializeField] string stateName;
    public string StateName 
    { 
        get { return stateName; }
    }

    public void SetUpAnimation(AbilityData data, Action finished)
    {
        Animator animator = data.GetUser().GetComponent<Animator>();

        AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

        if (overrideController != null && replacementClip != null)
        {
            // Check if the state exists in the Animator's controller
            if (overrideController[stateName] != null)
            {
                // Override the animation clip for the specified state
                overrideController[stateName] = replacementClip;

                // Assign the modified AnimatorOverrideController back to the Animator component
                animator.runtimeAnimatorController = overrideController;
            }
        }
        finished();
    }



}
