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
    [SerializeField] float timeTillDisable = 1f;
    private List<Collider> alreadyCollidedWith = new List<Collider>();
    private float timeCountDown;

    private void OnEnable()
    {
        timeCountDown = timeTillDisable;
        alreadyCollidedWith.Clear();
    }

    private void Update()
    {
        if(PlayerStateMachine.IsInteracting) { return; }
        timeCountDown -= Time.deltaTime;
        if(timeCountDown < 0)
        {
            timeCountDown = timeTillDisable;
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            return;
        }
        if (alreadyCollidedWith.Contains(other)) {return; }
        alreadyCollidedWith.Add(other);
        PlayerStateMachine.IsInteracting = true;
        IInteractable rayCastInteractive = other.GetComponent<IInteractable>();
        rayCastInteractive.HandleRaycastInteract(this);
        Vector3 lookPos = other.transform.position - PlayerStateMachine.transform.position;
        lookPos.y = 0f;
        PlayerStateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }
}

//Set the object layer to Interactable for more performance
public interface IInteractable
{
    void SetInteractionType();
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
