using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerController controller;

    public Animator animator;

    public float runSpeed = 40f;

    float f_hrzMove = 0f;
    bool b_jump, b_rot = false;

    // Update is called once per frame
    void Update()
    {
        //getaxisRaw if you want -1 0 1 get axis 0.0f 0.5f 1.0f and so on
        f_hrzMove = Input.GetAxis("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(f_hrzMove));

        if (Input.GetButtonDown("Jump"))
        {
            b_jump = true;
            animator.SetBool("IsJumping", true);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(f_hrzMove * Time.fixedDeltaTime, b_jump, b_rot);
        b_jump = false;
    }

}