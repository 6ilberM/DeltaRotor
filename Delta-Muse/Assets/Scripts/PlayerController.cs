using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //Components
    public Rigidbody2D rb2_MyBody;

    //Variables
    public RotationManager rm_Main;
    [Range(1, 80)] public float f_SpeedScalar = 1.0f;
    [Range(2, 14)] public int i_JumpScalar = 2;

    float currentTime = 0.0f;
    ///How long An object Rotates
    public float f_DesRDelta = 0.62f;

    //Make a check on this on the Rotation manager
    public bool b_DirChosen, b_RotDoOnce;

    int i_jumpCount;
    Quaternion qt_DesiredRot;
    
    //Overlap methods 
    ContactFilter2D Cfilter2d1;
    Collider2D[] overlapResults;
    //obj References

    private bool b_isgrounded;
    bool b_jumpL, b_HorizL, b_HasKey = false;

    public bool b_DeathRequest = false;

    // Use this for initialization
    void Start()
    {
        Cfilter2d1.useTriggers = true;
        Cfilter2d1.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        Cfilter2d1.useLayerMask = true;
        rm_Main = Object.FindObjectOfType<RotationManager>();
    }

    private void Awake()
    {
        rb2_MyBody = GetComponent<Rigidbody2D>();
        // rm_Main = Object.FindObjectOfType<RotationManager>();
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
        rm_Main.Rotate(b_DirChosen, qt_DesiredRot);
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

    //Make conditional Versions of this for enabling bigger rotations

    private void RotationSelect()
    {
        if (b_DirChosen == false)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                qt_DesiredRot = Quaternion.Euler(0, 0, -90 + rm_Main.transform.eulerAngles.z);
                b_DirChosen = true;
                rb2_MyBody.simulated = false;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                qt_DesiredRot = Quaternion.Euler(0, 0, 90 + rm_Main.transform.eulerAngles.z);
                b_DirChosen = true;
                rb2_MyBody.simulated = false;
            }
        }
    }

}
