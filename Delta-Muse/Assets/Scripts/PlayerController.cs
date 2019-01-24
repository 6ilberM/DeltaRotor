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

    [Range(1, 80)] public float f_SpeedScalar = 16.15f;
    [Range(2, 14)] public int i_JumpScalar = 2;
    [Range(15, 30)] public float MaxFallSpeed = 17.0f;

    //Temp
    float cutime;
    ///Determines how fast the player rotates
    public float plyrotspeed = 2;
    bool b_securitycheck;

    int RotId = 0;

    ///How long An object Rotates
    public float f_DesRDelta = 0.62f;


    //Make a check on this on the Rotation manager
    public bool b_DirChosen;

    int i_jumpCount;
    Quaternion qt_DesiredRot;

    //Overlap methods 
    ContactFilter2D Cfilter2d1;
    Collider2D[] overlapResults = null;

    //obj References


    private bool b_isgrounded;
    bool b_jumpL, b_HorizL = false;

    public bool b_DeathRequest = false;

    // Use this for initialization
    void Start()
    {

        Cfilter2d1.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        Cfilter2d1.useLayerMask = true;
        Cfilter2d1.useTriggers = true;

        rm_Main = Object.FindObjectOfType<RotationManager>();
    }

    private void Awake()
    {
        rb2_MyBody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        RotationSelect();

        //Check if we are on the ground
        if (Physics2D.Raycast(transform.position, Vector2.down, GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f, LayerMask.GetMask("Blocks"))
        || Physics2D.Raycast(transform.position, Vector2.down, GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.1f, LayerMask.GetMask("Blocks")))
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

    }



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
                    rb2_MyBody.AddForce(Vector2.up * i_JumpScalar * .7f, ForceMode2D.Impulse);
                    i_jumpCount++;

                    b_jumpL = false;
                    break;

                default:

                    b_jumpL = false;
                    break;
            }
        }

        //MaxFallSpeed
        rb2_MyBody.velocity = new Vector3(rb2_MyBody.velocity.x, Mathf.Clamp(rb2_MyBody.velocity.y, -MaxFallSpeed, 9000.0f), 0);

        //Rotate!
        rm_Main.Rotate(b_DirChosen, qt_DesiredRot);

        OrientSelfUp();
    }

    //Should be called in fixed time
    public void Move(float _velocityX, bool _Jump, bool _Rot)
    {
        if (!rm_Main.b_Rotate)
        {

        }
    }

    private void OrientSelfUp()
    {
        if (!b_DirChosen || !rm_Main.b_Rotate)
        {
            if (Mathf.Abs(qt_DesiredRot.eulerAngles.z
        - transform.localRotation.eulerAngles.z) <= 0.0001f)
            {
                transform.localRotation = rm_Main.transform.localRotation;
                cutime = 0.0f;
            }
            else
            {
                cutime += Time.fixedDeltaTime;

                Quaternion qtDir;

                switch (RotId)
                {
                    case 1:
                        qtDir = Quaternion.Euler(0, 0, 180 + 90);
                        transform.localRotation = Quaternion.Lerp(transform.localRotation, qtDir, cutime / plyrotspeed);

                        break;

                    case 3:
                        qtDir = Quaternion.Euler(0, 0, 90);
                        transform.localRotation = Quaternion.Lerp(transform.localRotation, qtDir, cutime / plyrotspeed);

                        break;

                    default:
                        qtDir = qt_DesiredRot;
                        transform.localRotation = Quaternion.Lerp(transform.localRotation, qtDir, cutime / plyrotspeed);
                        break;

                }
            }
        }
        else
        {
            cutime = 0;
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
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            i_jumpCount = 0;
        }
    }

    //Make conditional Versions of this for enabling bigger rotations
    public void RotationSelect()
    {
        if (b_DirChosen == false)
        {
            switch (RotId)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        qt_DesiredRot = Quaternion.Euler(0, 0, 180 + 90);
                        b_DirChosen = true;
                        rb2_MyBody.simulated = false;
                        RotId = 3;
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        qt_DesiredRot = Quaternion.Euler(0, 0, 90);
                        b_DirChosen = true;
                        rb2_MyBody.simulated = false;
                        RotId++;
                    }

                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        qt_DesiredRot = Quaternion.Euler(0, 0, 0);
                        b_DirChosen = true;
                        rb2_MyBody.simulated = false;
                        RotId = 0;
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        qt_DesiredRot = Quaternion.Euler(0, 0, 180);
                        b_DirChosen = true;
                        rb2_MyBody.simulated = false;
                        RotId++;
                    }
                    break;

                case 2:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        qt_DesiredRot = Quaternion.Euler(0, 0, 90);
                        b_DirChosen = true;
                        rb2_MyBody.simulated = false;
                        RotId--;
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        qt_DesiredRot = Quaternion.Euler(0, 0, 180 + 90);
                        b_DirChosen = true;
                        rb2_MyBody.simulated = false;
                        RotId++;
                    }
                    break;

                case 3:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        qt_DesiredRot = Quaternion.Euler(0, 0, 180);
                        b_DirChosen = true;
                        rb2_MyBody.simulated = false;
                        RotId--;
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        qt_DesiredRot = Quaternion.Euler(0, 0, 0);
                        b_DirChosen = true;
                        rb2_MyBody.simulated = false;
                        RotId = 0;
                    }
                    break;


                default:
                    //Unknown State
                    break;
            }
        }
    }

}
