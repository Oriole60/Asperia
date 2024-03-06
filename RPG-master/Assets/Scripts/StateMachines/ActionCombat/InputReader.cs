using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    [SerializeField] EventSignal openMenuSignal;


    public bool IsAttacking { get; private set; }
    public bool IsBlocking { get; private set; }
    public bool IsInteract { get; private set; }
    public bool IsUsingSlot { get; private set; }
    public Vector2 MovementValue { get; private set; }
    public float ChangeTargetValue { get; private set; }

    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action<int, GameObject> UseActionSlotSignal;

    public event Action<bool> OpenMenuEvent;


    private Controls controls;

    private bool isUsingMenu = false;

    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {

    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        TargetEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAttacking = true;
        }
        else if (context.canceled)
        {
            IsAttacking = false;
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsBlocking = true;
        }
        else if (context.canceled)
        {
            IsBlocking = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsInteract = true;
        }
        else if (context.canceled)
        {
            IsInteract = false;
        }
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        isUsingMenu = !isUsingMenu;
        OpenMenuEvent?.Invoke(isUsingMenu);
        openMenuSignal.Occurred(this.gameObject);
    }

    public void OnChangeTarget(InputAction.CallbackContext context)
    {
        ChangeTargetValue = context.ReadValue<float>();
    }

    public void OnActionSlot1(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        UseActionSlotSignal?.Invoke(0,gameObject);
    }

    public void OnActionSlot2(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        UseActionSlotSignal?.Invoke(1, gameObject);
    }

    public void OnActionSlot3(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        UseActionSlotSignal?.Invoke(2, gameObject);
    }

    public void OnActionSlot4(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        UseActionSlotSignal?.Invoke(3, gameObject);
    }
}
