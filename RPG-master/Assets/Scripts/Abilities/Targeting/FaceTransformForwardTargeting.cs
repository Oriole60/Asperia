using System;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Face Transform Forward Targeting", menuName = "Abilities/Targeting/FaceTransformFoward", order = 0)]
    public class FaceTransformForwardTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            Fighter fighter = data.GetUser().GetComponent<Fighter>();
            Health targetPoint = fighter.GetTarget();
            if(targetPoint != null)
            {
                data.SetTargetedPoint(targetPoint.transform.position);
                data.SetTargets(new GameObject[] { targetPoint.gameObject });
            }
            else
            {
                data.SetTargetedPoint(data.GetUser().transform.forward);
            }

            finished();
        }
    }
}
