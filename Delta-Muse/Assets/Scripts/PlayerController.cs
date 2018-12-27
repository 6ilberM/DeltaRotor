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
    [Range(2, 14)] public int i_JumpScalar = 2;

    //Make a check on this on the Rotation manager
    public bool b_DirChosen;

    int i_jumpCount;
    Quaternion qt_DesiredRot;
    //Overlap methods 
    ContactFilter2D Cfilter2d1;
    Collider2D[] overlapResults;
    //obj References
    public Transform Tr_obj;
    private bool b_isgrounded;
    bool b_jumpL, b_HorizL, b_HasKey = false;
    public bool b_DeathRequest = false;

    // Use this for initialization
    void Start()
    {
        Cfilter2d1.useTriggers = true;
        Cfilter2d1.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        Cfilter2d1.useLayerMask = true;
    }

    private void Awake()
    {
        rb2_MyBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RotationSelect();
        MoveInputListen();

        if (Physics2D.Raycast(transform.position, Vector2.down, GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f, LayerMask.GetMask("Blocks")))
        {
            b_isgrounded = true;
            // GetComponent<Animator>().SetBool("isGrounded", true);
        }
        else
        {
            b_isgrounded = false;
            // GetComponent<Animator>().SetBool("isGrounded", false)
        }

        //No Need to check every Tick.
        if (b_isgrounded)
        {
            i_jumpCount = 0;
        }
        if (GetComponent<BoxCollider2D>().OverlapCollider(Cfilter2d1, overlapResults) != 0)
        {
            Debug.Log(GetComponent<BoxCollider2D>().OverlapCollider(Cfilter2d1, overlapResults));
        }

    }
    bool b_negateonce = false;

    private void FixedUpdate()
    {

        if (b_HorizL)
        {
            rb2_MyBody.AddForce(new Vector2(Input.GetAxis("Horizontal") * f_SpeedScalar, 0));
        }

        if (b_jumpL)
        {
            if (rb2_MyBody.velocity.y < 0)
            {
                rb2_MyBody.velocity = new Vector2(rb2_MyBody.velocity.x, 0);
            }

            switch (i_jumpCount)
            {
                case 0:
                    rb2_MyBody.AddForce(Vector2.up * i_JumpScalar, ForceMode2D.Impulse);
                    i_jumpCount++;

                    b_jumpL = false;
                    break;

                case 1:
                    rb2_MyBody.AddForce(Vector2.up * i_JumpScalar * .5f, ForceMode2D.Impulse);
                    i_jumpCount++;

                    b_jumpL = false;
                    break;

                default:

                    b_jumpL = false;
                    break;
            }
        }

        //Rotate!
        RotationLogic();

    }

    private void RotationLogic()
    {
        if (Tr_obj != null && b_DirChosen == true)
        {

            float a, b;
            a = qt_DesiredRot.eulerAngles.z;
            b = Tr_obj.rotation.eulerAngles.z;
            //Close Enough? w/ thresholdCheck
            if (Mathf.Abs(a - b) <= 0.3f)
            {
                Tr_obj.rotation = qt_DesiredRot;
                b_DirChosen = false;
                rb2_MyBody.simulated = true;
                b_negateonce = false;
            }

            else
            {
                Tr_obj.rotation = Quaternion.Lerp(Tr_obj.rotation, qt_DesiredRot, f_RotSpeed);
                if (!b_negateonce)
                {
                    rb2_MyBody.velocity = Vector2.zero;
                    rb2_MyBody.simulated = false;
                    b_negateonce = true;
                }
            }
        }
    }

    private void MoveInputListen()
    {
        if (Input.GetButton("Horizontal"))
        {
            if (rb2_MyBody.velocity.magnitude < 400)
            {
                b_HorizL = true;
            }
            else if (Input.GetAxis("Horizontal") == 0)
            {
                b_HorizL = false;
            }
        }

        if (Input.GetButtonDown("Jump") && i_jumpCount < 2)
        {
            // rb2_MyBody.AddForce(new Vector2(0, -rb2_MyBody.velocity.y));
            b_jumpL = true;
        }
        else if (Input.GetButtonDown("Jump"))
        {
            //Do nothing
            Debug.Log("pastLimit");
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            i_jumpCount = 0;
        }
    }

    private void RotationSelect()
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

}
