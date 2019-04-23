using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{

    bool b_jump;

    Rigidbody2D m_RigidBody;

    private void Awake()
    {
        if (gameObject.GetComponent<Rigidbody2D>() != null)
        {
            m_RigidBody = gameObject.GetComponent<Rigidbody2D>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            b_jump = true;
        }
    }

    [SerializeField] float JumpHeight = 5;

    private void FixedUpdate()
    {
        if (b_jump)
        {
            float mygrav = m_RigidBody.gravityScale * Physics2D.gravity.y;


            var _jumpVelocity = (Mathf.Sqrt(Mathf.Abs(mygrav) * JumpHeight * 2.0f));

            m_RigidBody.AddForce(transform.up * _jumpVelocity * m_RigidBody.mass, ForceMode2D.Impulse);
            Debug.Log(_jumpVelocity);
            b_jump = false;
        }


        // _jumpGravity = -(2 * JumpHeight) / Mathf.Pow(TimeToJumpHeight , 2);
        // _jumpVelocity = Mathf.Abs(_jumpGravity) * TimeToJumpHeight ;

        // // Step update
        // stepMovement = (_velocity + Vector3.up * _gravity * Time.deltaTime * 0.5f) * Time.deltaTime;
        // transform.Translate(stepMovement);
        // _velocity.y += _gravity * Time.deltaTime;

        // // When jump button pressed,
        // _velocity.y = _jumpVelocity;

    }


}
