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

    [SerializeField] InteractMapping[] cursorMappings = null;

    [SerializeField] float shoutDistance = 2f;
    [SerializeField] PlayerStateMachine playerStateMachine;

    [field: SerializeField] public bool CanInteract { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!CanInteract) { return; }
        if(!playerStateMachine.InputReader.IsInteracting) { return; }
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
        foreach (RaycastHit hit in hits)
        {
            IInteractable rayCastInteractive = hit.collider.GetComponent<IInteractable>();
            if (rayCastInteractive == null) continue;

            rayCastInteractive.HandleRaycastInteract(this);
        }
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
