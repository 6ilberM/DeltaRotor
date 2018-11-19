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
    [Range(0, 1)] public float f_RotSpeed = 0.1f;
    [Range(100, 600)] public int i_JumpForce = 100;

    bool b_DirChosen;

    int i_jumpCount;
    Quaternion qt_DesiredRot;

    //obj References
    public Transform Tr_obj;
    private bool isgrounded;

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
        EnableRotation();
    }

    private void EnableRotation()
    {
        if (Tr_obj != null && b_DirChosen == false)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                qt_DesiredRot = Quaternion.Euler(0, 0, -90 + Tr_obj.eulerAngles.z);
                b_DirChosen = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                qt_DesiredRot = Quaternion.Euler(0, 0, 90 + Tr_obj.eulerAngles.z);
                b_DirChosen = true;
            }
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

        if (Input.GetButtonDown("Jump") && i_jumpCount < 2)
        {
            rb2_MyBody.AddForce(Vector2.up * i_JumpForce);
            i_jumpCount++;
        }

        if (isgrounded)
        {
            i_jumpCount = 0;
            isgrounded = false;
        }

        if (Tr_obj != null && b_DirChosen == true)
        {
            float a, b;
            a = qt_DesiredRot.eulerAngles.z;
            b = Tr_obj.rotation.eulerAngles.z;
            if (Mathf.Abs(a - b) <= .5)
            {
                Tr_obj.rotation = qt_DesiredRot;
                b_DirChosen = false;
            }
            else
            {
                Tr_obj.rotation = Quaternion.Slerp(Tr_obj.rotation, qt_DesiredRot, f_RotSpeed);
            }
        }
    }
}
