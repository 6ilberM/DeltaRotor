using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerController controller;

    [SerializeField] KeyCode m_rotRight = KeyCode.E;
    [SerializeField] KeyCode m_rotLeft = KeyCode.Q;



    public Animator animator;

    public float runSpeed = 40f;

    float f_hrzMove = 0f;
    bool b_jump, b_right, b_left = false;
    public bool m_jumpEnabled;

    // Update is called once per frame
    void Update()
    {
        //getaxisRaw if you want -1 0 1 get axis 0.0f 0.5f 1.0f and so on
        f_hrzMove = Input.GetAxis("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(f_hrzMove));

        if (Input.GetButtonDown("Jump") && m_jumpEnabled)
        {
            b_jump = true;
            animator.SetBool("IsJumping", true);
        }

        b_right = Input.GetKeyDown(m_rotRight);
        b_left = Input.GetKeyDown(m_rotLeft);
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(f_hrzMove * Time.fixedDeltaTime, b_jump, b_left, b_right);
        b_jump = false;
    }

}