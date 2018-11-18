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
            var este = Tr_obj.eulerAngles;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                b_DirChosen = true;
                qt_DesiredRot = Quaternion.Euler(0, 0, -90 + este.z);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                b_DirChosen = true;
                qt_DesiredRot = Quaternion.Euler(0, 0, 90 + este.z);
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

        if (Tr_obj != null)
        {
            if (b_DirChosen == true&&qt_DesiredRot != Tr_obj.rotation)
            {
                Tr_obj.rotation = Quaternion.Slerp(Tr_obj.rotation, qt_DesiredRot, f_RotSpeed);
            }
            else if (qt_DesiredRot == Tr_obj.rotation&&b_DirChosen==true)
            {
                b_DirChosen = false;
                qt_DesiredRot = new Quaternion();
            }

            
            string mystr = Tr_obj.rotation.ToString();
            mystr += " , ";
            mystr += qt_DesiredRot.ToString();
            Debug.Log(mystr);

        }
    }

}
