using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerInputController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] PlayerController controller;
#pragma warning restore 0649

    public InputAction JumpAction;
    public InputAction MoveAction;
    public InputActionMap gameplayActions;

    public Action onJump;
    public Action<bool> onRotate;
    public Animator animator;
    public float runSpeed = 40f;
    private float f_Horizontal = 0f;

    public bool m_jumpEnabled = true;

    [SerializeField] PlayerInput m_input;
    //DefaultplayerControls actions;

    //Milagros te amo!

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        JumpAction.performed += (InputAction.CallbackContext ctx) =>
        {
            onJump?.Invoke();
            animator.SetBool("IsJumping", true);
        };
        gameplayActions.actions[0].performed += RotLeft;
        gameplayActions.actions[1].performed += RotRight;
    }

    private void RotLeft(InputAction.CallbackContext _cbContext) { onRotate?.Invoke(false); }
    private void RotRight(InputAction.CallbackContext _cbContext) { onRotate?.Invoke(true); }

    private void OnEnable()
    {
        JumpAction.Enable();
        MoveAction.Enable();
        gameplayActions.Enable();
    }

    private void OnDisable()
    {
        JumpAction.Disable();
        MoveAction.Disable();
        gameplayActions.Disable();
    }

    //ToDo: Set Interact to another Button
    private void JumpInputAction(InputAction.CallbackContext obj)
    {
        onJump?.Invoke();
        animator.SetBool("IsJumping", true);
    }

    void Update()
    {
        f_Horizontal = MoveAction.ReadValue<float>() * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(f_Horizontal));
    }

    public void OnLanding() { animator.SetBool("IsJumping", false); }

    void FixedUpdate() { controller.Move(f_Horizontal * Time.fixedDeltaTime); }
}
