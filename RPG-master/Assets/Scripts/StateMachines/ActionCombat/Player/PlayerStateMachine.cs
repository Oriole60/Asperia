using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Combat;
using RPG.Dialogue;
using RPG.Shops;
using RPG.Stats;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public float DodgeDuration { get; private set; }
    [field: SerializeField] public float DodgeLength { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public BaseStats BaseStats { get; private set; }

    [field: SerializeField] public Fighter Fighter { get; private set; }
    [field: SerializeField] public PlayerConversant PlayerConversant { get; private set; }
    [field: SerializeField] public Shopper Shopper { get; private set; }
    [field: SerializeField] public Inventory Inventory { get; private set; }

    [SerializeField] private CinemachineInputProvider cinemachineInputProvider;

    public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;
    public Transform MainCameraTransform { get; private set; }
    public bool IsInteracting { get; set; } = false;


    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerFreeLookState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
        PlayerConversant.onConversationUpdated += OnInteracting;
        Shopper.OnIsShopping += OnInteracting;
        Inventory.OnPickupItem += OnInteracting;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
        PlayerConversant.onConversationUpdated -= OnInteracting;
        Shopper.OnIsShopping -= OnInteracting;
        Inventory.OnPickupItem -= OnInteracting;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this));
    }

    private void OnInteracting(bool isInteracting)
    {
        Cursor.lockState = isInteracting? CursorLockMode.None: CursorLockMode.Locked;
        Cursor.visible = isInteracting;
        IsInteracting = isInteracting;
        cinemachineInputProvider.enabled = !isInteracting;
    }

}
