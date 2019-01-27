using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //Components
    public Rigidbody2D m_rigidBody;

    //Variables
    public RotationManager rotManager;

    [Range(1, 80)] public float f_speedScalar = 16.15f;
    [Range(2, 14)] public int i_jumpScalar = 2;
    public float f_jumpForce = 300.0f;
    [Range(15, 30)] public float maxfallSpeed = 17.0f;

    //Temp
    float m_curentTime;

    public Quaternion m_PrevRot;
    private Vector3 m_Velocity = Vector3.zero;

    /// How much to smooth out the movement
    [Range(0, .3f)] [SerializeField] private float m_faSmoothing = .08f;

    ///Determines how fast the player rotates
    public float f_rotDuration = 0.3f;

    bool b_securityCheck;

    public bool b_ShouldSelfOrient = false;

    bool m_StoreRotation;
    Quaternion qtDir;


    ///How long An object Rotates

    //Make a check on this on the Rotation manager
    public bool b_dirChosen;

    int i_jumpCount;

    Quaternion qt_desiredRot;

    //Overlap methods 
    ContactFilter2D Cfilter2d1;
    Collider2D[] overlapResults = null;

    //obj References

    private bool b_isGrounded;
    bool b_jumpL, b_horizL, b_horizR = false;

    public bool b_DeathRequest = false;
    private bool m_FacingRight;

    // Use this for initialization
    void Start()
    {
        Cfilter2d1.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        Cfilter2d1.useLayerMask = true;
        Cfilter2d1.useTriggers = true;

        rotManager = Object.FindObjectOfType<RotationManager>();
    }

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if we are on the ground
        if (Physics2D.Raycast(transform.position, Vector2.down, GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f, LayerMask.GetMask("Blocks"))
        || Physics2D.Raycast(transform.position, Vector2.down, GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.1f, LayerMask.GetMask("Blocks")))
        {
            GetComponent<Animator>().SetBool("IsJumping", false);

            b_isGrounded = true;
            b_horizL = false;
            b_horizR = false;
            i_jumpCount = 0;

            // GetComponent<Animator>().SetBool("isGrounded", true);
        }
        else
        {
            // b_isGrounded = false;
            // GetComponent<Animator>().SetBool("isGrounded", false)
        }

        //No Need to check every Tick.
        if (!b_isGrounded)
        {
            leftrightRayChk();
        }

    }

    private void FixedUpdate()
    {
        //MaxFallSpeed
        m_rigidBody.velocity = new Vector3(m_rigidBody.velocity.x, Mathf.Clamp(m_rigidBody.velocity.y, -maxfallSpeed, 9000.0f), 0);

        //Rotate!
        rotManager.Rotate(b_dirChosen, qt_desiredRot);

        OrientSelfUp();
    }

    //Should be called in fixed time
    public void Move(float _velocityX, bool _Jump)
    {
        if (!rotManager.m_rotate)
        {
            Vector3 v3_targetVel = new Vector2(_velocityX * 10f, m_rigidBody.velocity.y);


            if (b_horizL && _velocityX > 0 || b_horizR && _velocityX < 0)
            {
                m_rigidBody.velocity = Vector3.SmoothDamp(m_rigidBody.velocity, v3_targetVel, ref m_Velocity, m_faSmoothing);
            }

            else if (b_horizL && b_horizR)
            {
                //trapped
            }

            else if (!b_horizL && !b_horizR)
            {
                m_rigidBody.velocity = Vector3.SmoothDamp(m_rigidBody.velocity, v3_targetVel, ref m_Velocity, m_faSmoothing);
            }


            // If the input is moving the player Left and the player is facing left...
            if (_velocityX > 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player Right and the player is facing right...
            else if (_velocityX < 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }

            //jumpLogic
            if (_Jump && i_jumpCount < 2)
            {
                // Add a vertical force to the player.
                if (m_rigidBody.velocity.y < 0)
                {
                    m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, 0);
                }

                if (i_jumpCount == 0)
                {
                    m_rigidBody.AddForce(new Vector2(0f, f_jumpForce), ForceMode2D.Impulse);
                    i_jumpCount++;
                }

                else if (i_jumpCount == 1)
                {
                    m_rigidBody.AddForce(new Vector2(0f, f_jumpForce * 0.7f), ForceMode2D.Impulse);
                    i_jumpCount++;
                }

                else
                {
                    //do nothing
                }
                b_isGrounded = false;
            }

            RotationSelect();
        }
    }

    private void OrientSelfUp()
    {
        if (!m_StoreRotation)
        {
            m_PrevRot = transform.localRotation;
            m_StoreRotation = true;
        }

        //If It no longer is rotating and should OrientSelf
        if (!rotManager.m_rotate && b_ShouldSelfOrient)
        {

            m_curentTime += Time.fixedDeltaTime;
            //Close Enough? w/ thresholdCheck
            if (m_curentTime > f_rotDuration)
            {
                transform.localRotation = qtDir;

                m_rigidBody.simulated = true;
                b_ShouldSelfOrient = false;
                m_StoreRotation = false;
                m_curentTime = 0.0f;
            }
            else
            {
                float t = m_curentTime / f_rotDuration;

                t = t * t * t * (t * (6f * t - 15f) + 10f);

                qtDir = rotManager.transform.localRotation;

                if (rotManager.rotationId == 1 || rotManager.rotationId == 3)
                {
                    qtDir = Quaternion.Inverse(qtDir);

                }
                else
                {
                }
                transform.localRotation = Quaternion.Slerp(m_PrevRot, qtDir, t);

            }
        }
    }

    void leftrightRayChk()
    {
        //Check Left 
        if (Physics2D.Raycast(transform.position, Vector2.left, GetComponent<BoxCollider2D>().bounds.extents.x + 0.2f, LayerMask.GetMask("Blocks"))
        || Physics2D.Raycast(transform.position, Vector2.left, GetComponent<CapsuleCollider2D>().bounds.extents.x + 0.2f, LayerMask.GetMask("Blocks")))
        {

            if (!b_horizL)
            {
                Debug.Log("hitting wallL");
            }

            b_horizL = true;
            // GetComponent<Animator>().SetBool("isGrounded", true);
        }
        else
        {
            b_horizL = false;
            // GetComponent<Animator>().SetBool("isGrounded", false)
        }


        //Check Right

        if (Physics2D.Raycast(transform.position, Vector2.right, GetComponent<BoxCollider2D>().bounds.extents.x + 0.2f, LayerMask.GetMask("Blocks"))
        || Physics2D.Raycast(transform.position, Vector2.right, GetComponent<CapsuleCollider2D>().bounds.extents.x + 0.2f, LayerMask.GetMask("Blocks")))
        {
            if (!b_horizR)
            {
                Debug.Log("hitting wallR");
            }
            b_horizR = true;


            // GetComponent<Animator>().SetBool("isGrounded", true);
        }
        else
        {
            b_horizR = false;
            // GetComponent<Animator>().SetBool("isGrounded", false)
        }
    }

    ///Flips Character
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;


        // Spin Renderer 
        // SpriteRenderer MyImage = gameObject.GetComponent<SpriteRenderer>();
        // MyImage.flipX = !MyImage.flipX;

        Vector3 theScale = transform.localScale;

        // Multiply the player's x local scale by -1. In case I attach more things to the player
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //Make conditional Versions of this for enabling bigger rotations
    public void RotationSelect()
    {
        if (rotManager.m_rotate == false)
        {
            // m_PrevRot = transform.localRotation;
            switch (rotManager.rotationId)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 180 + 90);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId = 3;
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 90);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId++;
                    }

                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 0);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId = 0;
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 180);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId++;
                    }
                    break;

                case 2:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 90);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId--;
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 180 + 90);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId++;
                    }
                    break;

                case 3:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 180);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId--;
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 0);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId = 0;
                    }
                    break;

                default:
                    //Unknown State
                    break;
            }
        }
    }

}
