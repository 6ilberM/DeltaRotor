using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //Components
    public Rigidbody2D m_rigidBody;

    //Variables
    public RotationManager rotManager;

    List<RotationArea> li_rotAreas = new List<RotationArea>();
    [SerializeField] float JumpHeight = 5;

    [Range(1, 80)] public float f_speedScalar = 16.15f;

    [Range(15, 30)] public float maxfallSpeed = 17.0f;

    //Temp
    float m_curentTime;

    Quaternion m_PrevRot;
    private Vector3 m_Velocity = Vector3.zero;

    /// How much to smooth out the movement
    [Range(0, .3f)] [SerializeField] private float m_faSmoothing = .08f;

    ///Determines how fast the player rotates
    [SerializeField] private float m_RotationDelay = 0.3f;

    float m_durationScalar = 1;

    bool b_securityCheck;

    public bool b_ShouldSelfOrient = false;

    bool m_StoreRotation;

    Quaternion qtDir;

    //Make a check on this on the Rotation manager
    public bool b_dirChosen;

    int i_jumpCount;

    Quaternion qt_desiredRot;

    //Overlap methods 
    ContactFilter2D Cfilter2d1;
    Collider2D[] overlapResults;

    //obj References

    //Events

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

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
        if (rotManager != transform.parent)
        {
            transform.SetParent(rotManager.transform);
        }
    }
    Animator m_animator;
    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = gameObject.GetComponent<Animator>();
        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }
    }

    Vector3 oldscale;


    float dt;

    bool m_StandUp;

    // Update is called once per frame
    void Update()
    {
        //Check if we are on the ground
        if (!rotManager.m_rotate)
        {
            groundRayCheck();
            if (m_animator.speed != 1)
            {
                m_animator.speed = 1;
            }
        }
        else
        {
            m_animator.speed = 0;
        }

        //No Need to check every Tick.
        if (!b_isGrounded)
        {
            wallRayCheck();
            // wallcheck();
        }

    }

    private void FixedUpdate()
    {
        if (!rotManager.m_rotate)
        {
            //Landing
            // LandingFeel();

            //MaxFallSpeed
            // m_rigidBody.velocity = new Vector3(m_rigidBody.velocity.x, Mathf.Clamp(m_rigidBody.velocity.y, -maxfallSpeed, 9000.0f), 0);

            //Rotate!
        }
        rotManager.Rotate(b_dirChosen, qt_desiredRot);

        OrientSelfUp();
    }

    ///Scales down and then back up quickly to improve the game feel


    private void LandingFeel()
    {
        if (m_StandUp)
        {

            Vector3 targetscale = new Vector3(oldscale.x * 2, oldscale.y * .5f, transform.localScale.z);
            // Vector2 NutargetScale = new Vector2(.55f, .86f);
            dt += Time.fixedDeltaTime;

            float f_Delay = 0.2f;
            if (dt > f_Delay * 2)
            {
                dt = 0;
                m_StandUp = false;
            }

            // t = t / .20f*2;
            if (dt < f_Delay)
            {
                float t = dt;
                t = t / f_Delay;
                t = (1 + (--t) * t * t);
                transform.localScale = Vector3.Lerp(oldscale, targetscale, t);
            }
            else
            {
                float t = dt - f_Delay;
                t = t / f_Delay;
                t = (1 + (--t) * t * t);
                GetComponent<CapsuleCollider2D>().size = Vector2.Lerp(GetComponent<CapsuleCollider2D>().size, new Vector2(.55f, .86f), 6.0f * Time.deltaTime);

                transform.localScale = Vector3.Lerp(targetscale, oldscale, t);
            }
        }
        else
        {
            GetComponent<CapsuleCollider2D>().direction = CapsuleDirection2D.Vertical;
            GetComponent<CapsuleCollider2D>().size = new Vector2(.55f, .86f);
        }
    }

    //Should be called in fixed time

    public void Move(float _velocityX, bool _Jump, bool _left, bool _right)
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

            //jumpLogic soon to be changed

            newJump(_Jump);

            RotationSelect(_left, _right);
        }
    }

    private void newJump(bool _Jump)
    {
        if (_Jump && i_jumpCount < 2)
        {
            // Add a vertical force to the player.
            if (m_rigidBody.velocity.y < 0)
            {
                m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, 0);

            }

            if (i_jumpCount == 0)
            {
                float mygrav = m_rigidBody.gravityScale * Physics2D.gravity.y;


                var _jumpVelocity = (Mathf.Sqrt(Mathf.Abs(mygrav) * JumpHeight * 2.0f));

                m_rigidBody.AddForce(m_rigidBody.transform.up * _jumpVelocity * m_rigidBody.mass, ForceMode2D.Impulse);

                i_jumpCount++;

            }

            else if (i_jumpCount == 1)
            {
                float mygrav = m_rigidBody.gravityScale * Physics2D.gravity.y;


                var _jumpVelocity = (Mathf.Sqrt(Mathf.Abs(mygrav) * JumpHeight * 2.0f));

                m_rigidBody.AddForce(m_rigidBody.transform.up * _jumpVelocity * .5f * m_rigidBody.mass, ForceMode2D.Impulse);

                i_jumpCount++;

            }
            else
            {
                //do nothing
            }

            b_isGrounded = false;
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
            if (m_curentTime > m_RotationDelay * DurationScalar)
            {
                transform.localRotation = qtDir;

                m_rigidBody.simulated = true;
                b_ShouldSelfOrient = false;
                m_StoreRotation = false;
                m_curentTime = 0.0f;
            }
            else
            {
                float t = m_curentTime / (m_RotationDelay * DurationScalar);


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

    void groundRayCheck()
    {
        bool wasgrounded = b_isGrounded;
        b_isGrounded = false;

        //Landed
        if (/*(Physics2D.Raycast(transform.position, Vector2.down, GetComponent<BoxCollider2D>() GetComponent<CapsuleCollider2D>().size.bounds.extents.y
        + 0.1f, LayerMask.GetMask("Blocks")) || */ (Physics2D.Raycast(transform.position, Vector2.down,
        GetComponent<CapsuleCollider2D>().bounds.extents.y + .6f, LayerMask.GetMask("Blocks"))) && (m_rigidBody.velocity.normalized.y <= 0))
        {
            b_isGrounded = true;

            b_horizL = false;
            b_horizR = false;
            i_jumpCount = 0;
            if (!wasgrounded)
            {
                OnLandEvent.Invoke();
                if (!m_StandUp)
                {
                    //Comment out if  no landfeel
                    // GetComponent<CapsuleCollider2D>().size = new Vector2(.9f, .6f);

                    // GetComponent<CapsuleCollider2D>().direction = CapsuleDirection2D.Horizontal;

                    m_StandUp = true;
                    // oldscale = new Vector2(1, 1);
                }

            }
        }
    }

    void wallRayCheck()
    {
        Vector3 pos2 = new Vector3(0, -.3f);
        //Check Left 
        if (Physics2D.Raycast(transform.position, Vector2.left, GetComponent<BoxCollider2D>().bounds.extents.x + 0.2f, LayerMask.GetMask("Blocks"))
        || Physics2D.Raycast(transform.position + pos2, Vector2.left, GetComponent<CapsuleCollider2D>().bounds.extents.x + 0.2f, LayerMask.GetMask("Blocks")))
        {

            if (!b_horizL)
            {
                // Debug.Log("hitting wallL");
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
        || Physics2D.Raycast(transform.position + pos2, Vector2.right, GetComponent<CapsuleCollider2D>().bounds.extents.x + 0.2f, LayerMask.GetMask("Blocks")))
        {
            if (!b_horizR)
            {
                // Debug.Log("hitting wallR");
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
        SpriteRenderer MyImage = gameObject.GetComponent<SpriteRenderer>();
        MyImage.flipX = !MyImage.flipX;

        // //Spin the scale
        //         Vector3 theScale = transform.localScale;

        //         // Multiply the player's x local scale by -1. In case I attach more things to the player
        //         theScale.x *= -1;
        //         transform.localScale = theScale;
    }

    //Make conditional Versions of this for enabling bigger rotations
    public bool canrotsingle;

    public float DurationScalar
    {
        get
        {
            return m_durationScalar;
        }

        set
        {
            m_durationScalar = value;
        }
    }

    public List<RotationArea> li_rotationAreas
    {
        get
        {
            return li_rotAreas;
        }

        set
        {
            li_rotAreas = value;
        }
    }

    public void RotationSelect(bool _left, bool _right)
    {
        if (rotManager.m_rotate == false && !canrotsingle)
        {
            // m_PrevRot = transform.localRotation;
            switch (rotManager.rotationId)
            {
                case 0:
                    if (_right)
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 180 + 90);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId = 3;
                    }

                    if (_left)
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 90);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId++;
                    }

                    break;
                case 1:
                    if (_right)
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 0);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId = 0;
                    }

                    if (_left)
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 180);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId++;
                    }
                    break;

                case 2:
                    if (_right)
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 90);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId--;
                    }

                    if (_left)
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 180 + 90);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId++;
                    }
                    break;

                case 3:
                    if (_right)
                    {
                        qt_desiredRot = Quaternion.Euler(0, 0, 180);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId--;
                    }

                    if (_left)
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
        else if (canrotsingle)
        {
            if (_left)
            {
                for (int i = 0; i < li_rotationAreas.Count; i++)
                {
                    li_rotationAreas[i].RotSelect(0);
                }
            }

            if (_right)
            {
                for (int i = 0; i < li_rotationAreas.Count; i++)
                {
                    li_rotationAreas[i].RotSelect(1);
                }
            }
        }
        else
        {
            //do nothing
        }
    }

}
