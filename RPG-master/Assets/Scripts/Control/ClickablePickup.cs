using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable,IInteractable
    {
        [SerializeField] 
        Pickup pickup;
        [SerializeField] Animator animator;
        private const string IS_ACTIVE_ANIM = "isActive";

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp())
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.FullPickup;
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pickup.PickupItem();
            }
            return true;
        }

        public void HandleRaycastInteract(PlayerInteraction callingController)
        {
            pickup.PickupItem();
        }

        public void SetInteractionType(bool isActive)
        {
            animator.SetBool(IS_ACTIVE_ANIM, isActive);
        }
    }
}
