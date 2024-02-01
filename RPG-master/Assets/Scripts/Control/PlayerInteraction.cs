using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Set the player Layer to PLayer for more performance
public class PlayerInteraction : MonoBehaviour
{
    [System.Serializable]
    struct InteractMapping
    {
        public InteractionType type;
        public Image texture;
    }

    [field: SerializeField] public PlayerStateMachine PlayerStateMachine { get; private set; }
    private List<Collider> alreadyCollidedWith = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyCollidedWith.Contains(other)) {return; }
        alreadyCollidedWith.Add(other);
        IInteractable rayCastInteractive = other.GetComponent<IInteractable>();
        rayCastInteractive.HandleRaycastInteract(this);
    }
}

//Set the object layer to Interactable for more performance
public interface IInteractable
{
    InteractionType GetCursorType();
    void HandleRaycastInteract(PlayerInteraction callingController);
}

public enum InteractionType
{
    None,
    Pickup,
    FullPickup,
    Dialogue,
    Shop
}
