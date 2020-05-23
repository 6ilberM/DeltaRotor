using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputController))]

public class PlayerController : MonoBehaviour
{

    //Components
    public Rigidbody2D m_rb;
    private CapsuleCollider2D m_capsuleCollider;

    //References
    public RotationManager m_CurrentRotation;
    private PlayerInputController m_inputController;

    //Variables
    [HideInInspector] public List<RotationArea> RotAreaList;

    [Header("Customizable Fields")]
    [SerializeField] private float JumpHeight = 5;
    [SerializeField] private float f_runSpeed = 40f;

    private float m_curentTime;
    private Quaternion m_PrevRot;

    private Vector2 m_velRef = Vector2.zero;

    /// How much to smooth out the movement
    [Range(0, .3f)] [SerializeField] private float m_faSmoothing = .08f;

    ///<summary>Determines how much the player will take to rotate up?</summary>
    [SerializeField] private float m_RotationDelay = 0.3f;

    private bool m_StoreRotation;
    Quaternion m_QuatDirection;
    public bool b_dirChosen;

    private Quaternion m_desiredRotation;
    private ContactFilter2D m_contactFilter;
    private Collider2D[] overlapResults;
    private SpriteRenderer m_SpriteRenderer;
    private Animator m_animator;
    private Vector3 m_InitialScale;
    private CapsuleCollider2D m_collider2D;
    private float m_dt;
    private bool m_StandUp;

    [Header("Events")]
    [Space(10)]
    public UnityEvent OnLandEvent;
    public bool b_SelfOrient = false;

    //Make conditional Versions of this for enabling bigger rotations
    public bool b_RotateSingle;
    private bool b_isGrounded;
    private bool b_horizL, b_horizR = false;
    public bool b_DeathRequest = false;
    private bool m_faceRight = false;
    private bool b_hasJumped = false;
    public float m_rotationDuration;

    private void Awake()
    {
        m_inputController = GetComponent<PlayerInputController>();

        m_inputController.onJump += OnJump;
        m_inputController.onRotate += OnRotate;
        m_InitialScale = transform.localScale;

        //Physics
        m_rb = GetComponent<Rigidbody2D>();
        m_capsuleCollider = GetComponent<CapsuleCollider2D>();

        //Anims
        m_animator = gameObject.GetComponent<Animator>();
        m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (OnLandEvent == null) { OnLandEvent = new UnityEvent(); }
    }

    private void Start()
    {
        m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        m_contactFilter.useLayerMask = true;
        m_contactFilter.useTriggers = true;

        m_CurrentRotation = transform.parent.GetComponent<RotationManager>();
        if (m_CurrentRotation != transform.parent) { transform.SetParent(m_CurrentRotation.transform); }
    }


    private void OnDestroy()
    {
        m_inputController.onJump -= OnJump;
        m_inputController.onRotate -= OnRotate;
    }

    private void OnRotate(bool _LeftOrRight)
    {
        if (!ReferenceEquals(m_CurrentRotation, null) && b_isGrounded)
        {
            m_desiredRotation = GetTargetRotation(_LeftOrRight);
            StartCoroutine(m_CurrentRotation.RotateWhile(m_desiredRotation));
        }
    }

    private void OnJump() { if (!b_hasJumped) { Jump(); } }


    void Update()
    {
        if (!m_CurrentRotation.m_rotate)
        {
            GroundRayCheck();
            if (m_animator.speed != 1) { m_animator.speed = 1; }
        }
        else { m_animator.speed = 0; }

        if (!b_isGrounded) { WallRayCheck(); }
    }

    private void LateUpdate()
    {
        if (!m_CurrentRotation.m_rotate) { OrientUpNew(); }
    }

    private void OrientUpNew()
    {
        if (b_SelfOrient)
        {
            if (Mathf.Abs(Quaternion.Dot(transform.rotation, Quaternion.identity)) >= 0.992126)
            {
                if (b_SelfOrient)
                {
                    this.transform.rotation = Quaternion.identity;
                    m_rb.simulated = true;
                    b_SelfOrient = false;
                }
            }
            else
            {
                Quaternion look = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, look, 5.65f*Time.deltaTime);
            }
        }
    }

    private void OnLandingCustom()
    {
        if (m_StandUp)
        {
            Vector3 targetscale = new Vector3(m_InitialScale.x * 2, m_InitialScale.y * .5f, transform.localScale.z);
            // Vector2 NutargetScale = new Vector2(.55f, .86f);
            m_dt += Time.fixedDeltaTime;

            float f_Delay = 0.2f;
            if (m_dt > f_Delay * 2)
            {
                m_dt = 0;
                m_StandUp = false;
            }

            // t = t / .20f*2;
            if (m_dt < f_Delay)
            {
                float t = m_dt;
                t = t / f_Delay;
                t = (1 + (--t) * t * t);
                transform.localScale = Vector3.Lerp(m_InitialScale, targetscale, t);
            }
            else
            {
                float t = m_dt - f_Delay;
                t = t / f_Delay;
                t = (1 + (--t) * t * t);
                m_capsuleCollider.size = Vector2.Lerp(m_capsuleCollider.size, new Vector2(.55f, .86f), 6.0f * Time.deltaTime);
                transform.localScale = Vector3.Lerp(targetscale, m_InitialScale, t);
            }
        }
        else
        {
            m_capsuleCollider.direction = CapsuleDirection2D.Vertical;
            m_capsuleCollider.size = new Vector2(.55f, .86f);
        }
    }
float horizDirection;
    public void Move(float _horizontalAxis)
    {
        horizDirection=_horizontalAxis;
        if (!m_CurrentRotation.m_rotate)
        {
            float _maxfall = 35f;
            // _maxfall = Mathf.Lerp(m_rb.velocity.y, _maxfall, Time.fixedDeltaTime * _maxfall);
            m_rb.velocity = new Vector2(m_rb.velocity.x, Mathf.Clamp(Mathf.Abs(m_rb.velocity.y), 0, _maxfall) * Mathf.Sign(m_rb.velocity.y));

            Vector2 _targetVelocity = (Vector2)(transform.right * (f_runSpeed * _horizontalAxis * Time.fixedDeltaTime)) + (Vector2.up * m_rb.velocity.y);

            if (b_horizL && _horizontalAxis > 0 || b_horizR && _horizontalAxis < 0)
            {
                m_rb.velocity = Vector2.SmoothDamp(m_rb.velocity, _targetVelocity, ref m_velRef, m_faSmoothing);
            }
            else if (!b_horizL && !b_horizR)
            {
                m_rb.velocity = Vector2.SmoothDamp(m_rb.velocity, _targetVelocity, ref m_velRef, m_faSmoothing);
            }

            if (_horizontalAxis > 0 && m_faceRight) { FlipSpriteDirection(); }
            else if (_horizontalAxis < 0 && !m_faceRight) { FlipSpriteDirection(); }
        }
    }

    //ToDo: Improve The New Jump  so that you can do the Mario-Esque Held Button Jump!
    private void Jump()
    {
        float _JumpForce = GetJumpForceAtHeight();
        float xforce = (Mathf.Abs(m_rb.velocity.x) > 0 ? Mathf.Sign(horizDirection)*Mathf.Abs(horizDirection) : 0) * 1.2f;

        if (m_rb.velocity.y < 0) { m_rb.velocity = new Vector2(m_rb.velocity.x, 0); }

        m_rb.AddForce((m_rb.transform.up * _JumpForce) + (m_rb.transform.right * xforce), ForceMode2D.Impulse);
        b_hasJumped = true;
        b_isGrounded = false;

    }

    private float GetJumpForceAtHeight()
    {
        return Mathf.Sqrt(Mathf.Abs(m_rb.gravityScale * Mathf.Pow(m_rb.mass, 2) * Physics2D.gravity.y * JumpHeight * 2));
    }

    // ToDo: OnOrientPlayerUp Re enable movement and so no no more checking in Update

    ///<summary>
    ///Aligns Actor to whatever the current upwards Vector is, 
    ///</summary>
    private void OrientPlayerUp()
    {
        //ToDo: Add upwards Vector & a cache for normal vector
        if (!m_StoreRotation)
        {
            m_PrevRot = transform.localRotation;
            m_StoreRotation = true;
        }

        if (!m_CurrentRotation.m_rotate && b_SelfOrient)
        {
            m_curentTime += Time.fixedDeltaTime;

            if (m_curentTime > m_RotationDelay * m_rotationDuration)
            {
                transform.localRotation = m_QuatDirection;
                m_rb.simulated = true;
                b_SelfOrient = false;
                m_StoreRotation = false;
                m_curentTime = 0.0f;
            }
            else
            {
                float t = m_curentTime / (m_RotationDelay * m_rotationDuration);

                t = t * t * t * (t * (6f * t - 15f) + 10f);

                m_QuatDirection = m_CurrentRotation.transform.localRotation;

                if (m_CurrentRotation.rotationId == 1 || m_CurrentRotation.rotationId == 3) { m_QuatDirection = Quaternion.Inverse(m_QuatDirection); }

                transform.localRotation = Quaternion.Slerp(m_PrevRot, m_QuatDirection, t);
            }
        }
    }

    void GroundRayCheck()
    {
        bool wasgrounded = b_isGrounded;
        b_isGrounded = false;

        //Landed
        if (/*(Physics2D.Raycast(transform.position, Vector2.down, GetComponent<BoxCollider2D>() GetComponent<CapsuleCollider2D>().size.bounds.extents.y
        + 0.1f, LayerMask.GetMask("Blocks")) || */ (Physics2D.Raycast(transform.position, -transform.up,
        GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.75f, LayerMask.GetMask("Blocks"))) && (m_rb.velocity.normalized.y <= 0))
        {
            b_isGrounded = true;
            b_hasJumped = false;

            b_horizL = false;
            b_horizR = false;
            if (!wasgrounded)
            {
                OnLandEvent.Invoke();
                if (!m_StandUp)
                {
                    //Comment out if  no landfeel
                    // m_capsuleCollider.size = new Vector2(.9f, .6f);
                    // m_capsuleCollider.direction = CapsuleDirection2D.Horizontal;

                    m_StandUp = true;
                    // oldscale = new Vector2(1, 1);
                }

            }
        }
    }
// ToDo: Cache GetCompoenent Calls for box and Capsule Colliders.
    void WallRayCheck()
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

    private void FlipSpriteDirection()
    {
        m_faceRight = !m_faceRight;
        m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;
    }

    public void ProcessRotation(bool b_direction)
    {
        if (m_CurrentRotation.m_rotate == false && !b_RotateSingle)
        {
            b_dirChosen = true;
            m_rb.simulated = false;
            // m_PrevRot = transform.localRotation;
            switch (m_CurrentRotation.rotationId)
            {
                case 0:
                    if (b_direction)
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 180 + 90);
                        m_CurrentRotation.rotationId = 3;
                    }

                    else
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 90);
                        m_CurrentRotation.rotationId++;
                    }

                    break;
                case 1:
                    if (b_direction)
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 0);
                        m_CurrentRotation.rotationId = 0;
                    }
                    else
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 180);
                        m_CurrentRotation.rotationId++;
                    }
                    break;

                case 2:
                    if (b_direction)
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 90);
                        m_CurrentRotation.rotationId--;
                    }

                    else
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 180 + 90);
                        m_CurrentRotation.rotationId++;
                    }
                    break;

                case 3:
                    if (b_direction)
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 180);
                        m_CurrentRotation.rotationId--;
                    }

                    else
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 0);

                        m_CurrentRotation.rotationId = 0;
                    }
                    break;

                default:
                    //Unknown State
                    break;
            }
        }
        else if (b_RotateSingle)
        {
            if (b_direction) { for (int i = 0; i < RotAreaList.Count; i++) { RotAreaList[i].SelectRotation(1); } }

            else { for (int i = 0; i < RotAreaList.Count; i++) { RotAreaList[i].SelectRotation(0); } }
        }
    }

    Quaternion GetTargetRotation(bool _dir)
    {
        Quaternion rot;
        b_dirChosen = true;
        m_rb.simulated = false;

        if (_dir) { rot = m_CurrentRotation.transform.rotation * Quaternion.Euler(0, 0, 90); }
        else { rot = m_CurrentRotation.transform.rotation * Quaternion.Euler(0, 0, -90); }

        return rot;
    }
}
