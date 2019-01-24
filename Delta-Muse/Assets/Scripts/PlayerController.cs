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
    float timeElapsed;
    private Vector3 m_Velocity = Vector3.zero;

    /// How much to smooth out the movement
    [Range(0, .3f)] [SerializeField] private float m_faSmoothing = .05f;

    ///Determines how fast the player rotates
    public float rotSpeed = 2;

    bool b_securityCheck;


    ///How long An object Rotates
    public float f_rotationDelay = 0.62f;

    //Make a check on this on the Rotation manager
    public bool b_dirChosen;

    int i_jumpCount;

    Quaternion qt_desiredRot;

    //Overlap methods 
    ContactFilter2D Cfilter2d1;
    Collider2D[] overlapResults = null;

    //obj References

    private bool b_isGrounded;
    bool b_jumpL, b_HorizL = false;

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
        RotationSelect();

        //Check if we are on the ground
        if (Physics2D.Raycast(transform.position, Vector2.down, GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f, LayerMask.GetMask("Blocks"))
        || Physics2D.Raycast(transform.position, Vector2.down, GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.1f, LayerMask.GetMask("Blocks")))
        {
            b_isGrounded = true;
            // GetComponent<Animator>().SetBool("isGrounded", true);
        }
        else
        {
            b_isGrounded = false;
            // GetComponent<Animator>().SetBool("isGrounded", false)
        }
        //No Need to check every Tick.
        if (b_isGrounded)
        {
            i_jumpCount = 0;
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
    public void Move(float _velocityX, bool _Jump, bool _Rot)
    {
        if (!rotManager.b_Rotate)
        {
            Vector3 v3_targetVel = new Vector2(_velocityX * 10f, m_rigidBody.velocity.y);
            m_rigidBody.velocity = Vector3.SmoothDamp(m_rigidBody.velocity, v3_targetVel, ref m_Velocity, m_faSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (_velocityX > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (_velocityX < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }

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
        }
    }

    private void OrientSelfUp()
    {
        if (!b_dirChosen || !rotManager.b_Rotate)
        {
            if (Mathf.Abs(qt_desiredRot.eulerAngles.z
        - transform.localRotation.eulerAngles.z) <= 0.0001f)
            {
                transform.localRotation = rotManager.transform.localRotation;
                timeElapsed = 0.0f;
            }
            else
            {
                timeElapsed += Time.fixedDeltaTime;

                Quaternion qtDir;

                switch (rotManager.rotationId)
                {
                    case 1:
                        qtDir = Quaternion.Euler(0, 0, 180 + 90);
                        transform.localRotation = Quaternion.Lerp(transform.localRotation, qtDir, timeElapsed / rotSpeed);

                        break;

                    case 3:
                        qtDir = Quaternion.Euler(0, 0, 90);
                        transform.localRotation = Quaternion.Lerp(transform.localRotation, qtDir, timeElapsed / rotSpeed);

                        break;

                    default:
                        qtDir = qt_desiredRot;
                        transform.localRotation = Quaternion.Lerp(transform.localRotation, qtDir, timeElapsed / rotSpeed);
                        break;
                }
            }
        }
        else
        {
            timeElapsed = 0;
        }

    }


    ///Flips Character
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //Make conditional Versions of this for enabling bigger rotations
    public void RotationSelect()
    {
        if (b_dirChosen == false)
        {
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
