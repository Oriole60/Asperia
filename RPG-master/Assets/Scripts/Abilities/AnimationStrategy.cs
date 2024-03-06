using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animation", menuName = "Abilities/AnimationStrategy/Animation", order = 0)]
public class AnimationStrategy : ScriptableObject
{
    [SerializeField] AnimationClip replacementClip;
    [SerializeField] string stateName;

    public void SetUpAnimation(AbilityData data, Action finished)
    {
        Animator animator = data.GetUser().GetComponent<Animator>();

        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

        Debug.Log($"overrideController is null  {overrideController is null }");
        if (overrideController != null)
        {
            AnimatorOverrideController newOverrideController = new AnimatorOverrideController(overrideController);
            // Check if the state exists in the Animator's controller

            // Override the animation clip for the specified state and animation
            newOverrideController["UseSlot"] = replacementClip;

            // Assign the modified AnimatorOverrideController back to the Animator component
            animator.runtimeAnimatorController = newOverrideController;
            
        }
        else
        {
            var animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;

            animatorOverrideController["UseSlot"] = replacementClip;
        }
        var overr8ideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        Debug.Log($"overrideController is null  {overr8ideController is null }");
        finished();
    }


}
