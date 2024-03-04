using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Trigger Animation Effect", menuName = "Abilities/Effects/Trigger Animation", order = 0)]
    public class TriggerAnimationEffect : EffectStrategy
    {
        [SerializeField] string animationTrigger;
        [SerializeField] AnimationClip replacementClip;
        [SerializeField] string stateName;


        public override void StartEffect(AbilityData data, Action finished)
        {
            Animator animator = data.GetUser().GetComponent<Animator>();

            AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (overrideController != null && animationTrigger != null && replacementClip != null)
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
            animator.SetTrigger(animationTrigger);
            finished();
        }
    }
}

