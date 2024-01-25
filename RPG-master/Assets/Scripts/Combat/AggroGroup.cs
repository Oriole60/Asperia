using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] Fighter[] fighters;
        [SerializeField] bool activateOnStart = false;

        private void Start() {
            Activate(activateOnStart);
        }

        public void Activate(bool shouldActivate)
        {
            foreach (Fighter fighter in fighters)
            {
                Target target = fighter.GetComponent<Target>();
                if (target != null)
                {
                    target.enabled = shouldActivate;
                }
                fighter.enabled = shouldActivate;
            }
        }
    }
}