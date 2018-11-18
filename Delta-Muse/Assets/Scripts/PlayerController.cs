using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //Components
    Rigidbody2D rb2_MyBody;

    //Variables
    [Range(1, 80)] public float f_SpeedScalar = 1.0f;
    [Range(1, 80)] public float f_RotationSpeed = 1.0f;
    [Range(100, 600)] public int i_JumpForce = 100;

    bool b_rotationComplete;

    //Misc

    //obj References
    public Transform Rotatorobj;

    // Use this for initialization
    void Start()
    {

    }

    private void Awake()
    {
        rb2_MyBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateParent();
    }

    private void RotateParent()
    {
        if (Rotatorobj != null)
        {
            Vector3 RotationAxis = new Vector3(0, 0, 1);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Rotatorobj.Rotate(RotationAxis, 90);
                Debug.Log(Rotatorobj.rotation);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Rotatorobj.Rotate(-RotationAxis, 90);
            }

        }
        else
        {
            //do nothing
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Horizontal"))
        {
            if (rb2_MyBody.velocity.magnitude < 400)
            {
                rb2_MyBody.AddForce(new Vector2(Input.GetAxis("Horizontal") * f_SpeedScalar, 0));
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            rb2_MyBody.AddForce(Vector2.up * i_JumpForce);
        }

    }


}
