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

    private bool isIntertacting;

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyCollidedWith.Contains(other)) {return; }
        if(!other.TryGetComponent<IInteractable>(out IInteractable rayCastInteractive)) { return; }
        alreadyCollidedWith.Add(other);
        rayCastInteractive.SetInteractionType(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if(!PlayerStateMachine.InputReader.IsInteract) { return; }
        if (!other.TryGetComponent<IInteractable>(out IInteractable rayCastInteractive)) { return; }
        if(PlayerStateMachine.IsInteracting) { return; }
        rayCastInteractive.HandleRaycastInteract(this);
        Vector3 lookPos = other.transform.position - PlayerStateMachine.transform.position;
        lookPos.y = 0f;
        PlayerStateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<IInteractable>(out IInteractable rayCastInteractive)) { return; }
        rayCastInteractive.SetInteractionType(false);
        if (alreadyCollidedWith.Contains(other))
        {
            alreadyCollidedWith.Remove(other);
        }
    }
}

//Set the object layer to Interactable for more performance
public interface IInteractable
{
    void SetInteractionType(bool isActive);
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
