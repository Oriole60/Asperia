using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private bool shouldFade;
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
    {
        this.shouldFade = shouldFade;
    }

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.PlayerConversant.onConversationUpdated += OnInteracting;
        stateMachine.Shopper.OnIsShopping += OnInteracting;
        stateMachine.Inventory.OnPickupItem += OnInteracting;
        stateMachine.InputReader.OpenMenuEvent += OnInteracting;
        stateMachine.InputReader.UseActionSlotSignal += OnUseSlot;


        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f);

        if (shouldFade)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        }
        else
        {
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
        }
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.IsInteracting)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f);
            return;
        }

        if (stateMachine.InputReader.IsAttacking )
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.PlayerConversant.onConversationUpdated -= OnInteracting;
        stateMachine.Shopper.OnIsShopping -= OnInteracting;
        stateMachine.Inventory.OnPickupItem -= OnInteracting;
        stateMachine.InputReader.OpenMenuEvent -= OnInteracting;
        stateMachine.InputReader.UseActionSlotSignal -= OnUseSlot;

    }

    private void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) { return; }

        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    private void OnJump()
    {
        if (stateMachine.IsInteracting)
        {
            return;
        }
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    private void OnUseSlot(int index, GameObject user)
    {
        if (stateMachine.IsInteracting)
        {
            return;
        }
        if(stateMachine.ActionStore.GetAction(index) == null) { return; }
        stateMachine.SwitchState(new PlayerUseSlotState(stateMachine,index,user));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y +
            right * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping);
    }

    private void OnInteracting(bool isInteracting)
    {
        Cursor.lockState = isInteracting ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isInteracting;
        stateMachine.IsInteracting = isInteracting;
        stateMachine.CinemachineInputProvider.enabled = !isInteracting;
    }

}
