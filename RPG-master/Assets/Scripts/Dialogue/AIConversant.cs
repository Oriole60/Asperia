using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable,IInteractable
    {
        [SerializeField] Dialogue dialogue = null;
        [SerializeField] string conversantName;
        [SerializeField] Health health;

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (dialogue == null)
            {
                return false;
            }

            if (health && health.IsDead()) return false;

            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
            }
            return true;
        }

        public string GetName()
        {
            return conversantName;
        }

        InteractionType IInteractable.GetCursorType()
        {
            return InteractionType.Dialogue;
        }

        public void HandleRaycastInteract(PlayerInteraction callingController)
        {
            if (dialogue == null)
            {
                return;
            }

            Health health = GetComponent<Health>();
            if (health && health.IsDead()) return;
            callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerInteraction>(out PlayerInteraction playerInteraction))
            {
                playerInteraction.CanInteract = true;
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerInteraction>(out PlayerInteraction playerInteraction))
            {
                playerInteraction.CanInteract = false;
            }
        }
    }
}