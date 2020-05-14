using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] PlayerController controller;
#pragma warning restore 0649

    //[SerializeField] Key m_rotRight = Key.E;
    //[SerializeField] Key m_rotLeft = Key.Q;

    public InputAction JumpAction;
    public InputAction MoveAction;

    public InputActionMap gameplayActions;

    public Animator animator;
    public float runSpeed = 40f;

    private float f_Horizontal = 0f;
    private bool b_jump, b_right, b_left = false;
    public bool m_jumpEnabled;

    [SerializeField] PlayerInput m_input;
    //DefaultplayerControls actions;

    private void Awake()
    {
        JumpAction.performed += JumpAction_performed;
        gameplayActions.actions[0].performed += RotLeft;
        gameplayActions.actions[1].performed += RotRight;
    }

    private void RotLeft(InputAction.CallbackContext obj) { b_left = true; }

    private void RotRight(InputAction.CallbackContext obj) { b_right = true; }

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

    private void JumpAction_performed(InputAction.CallbackContext obj)
    {
        if (m_jumpEnabled)
        {
            b_jump = true;
            animator.SetBool("IsJumping", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        f_Horizontal = MoveAction.ReadValue<float>() * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(f_Horizontal));
    }

    public void OnLanding() { animator.SetBool("IsJumping", false); }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(f_Horizontal * Time.fixedDeltaTime, b_jump, b_left, b_right);
        b_jump = false;
        b_left = false;
        b_right = false;
    }
}