using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputController))]

public class PlayerController : MonoBehaviour
{
    //Components
    public Rigidbody2D m_rigidBody;

    //References
    public RotationManager rotManager;
    private PlayerInputController m_inputController;

    //Variables
    public List<RotationArea> RotAreaList;

    [SerializeField] private float JumpHeight = 5;
    private float m_curentTime;
    private Quaternion m_PrevRot;
    private Vector3 m_Velocity = Vector3.zero;

    /// How much to smooth out the movement
    [Range(0, .3f)] [SerializeField] private float m_faSmoothing = .08f;

    ///Determines how fast the player rotates
    [SerializeField] private float m_RotationDelay = 0.3f;

    private bool m_StoreRotation;

    Quaternion m_QuatDirection;

    //Make a check on this on the Rotation manager (Should be a Callback)
    public bool b_dirChosen;
    private int m_JumpCount;
    private Quaternion m_desiredRotation;
    private ContactFilter2D m_contactFilter;
    private Collider2D[] overlapResults;

    private Vector3 oldscale;

    private float dt;
    private bool m_StandUp;
    //obj References
    private Animator m_animator;

    [Header("Events")]
    [Space(10)]
    public UnityEvent OnLandEvent;
    public bool b_SelfOrient = false;

    //Make conditional Versions of this for enabling bigger rotations
    public bool b_CanRotateSingle;

    private bool b_isGrounded;
    private bool b_horizL, b_horizR = false;
    public bool b_DeathRequest = false;
    private bool m_faceRight;
    public float m_DurationScalar;

    private void Start()
    {
        m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        m_contactFilter.useLayerMask = true;
        m_contactFilter.useTriggers = true;

        rotManager = UnityEngine.Object.FindObjectOfType<RotationManager>();
        if (rotManager != transform.parent) { transform.SetParent(rotManager.transform); }
    }

    private void Awake()
    {
        m_inputController = GetComponent<PlayerInputController>();
        m_inputController.onJump += OnJump;
        m_inputController.onRotate += OnRotate;
        oldscale = transform.localScale;
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = gameObject.GetComponent<Animator>();

        if (OnLandEvent == null) { OnLandEvent = new UnityEvent(); }
    }

    private void OnDestroy()
    {
        m_inputController.onJump -= OnJump;
        m_inputController.onRotate -= OnRotate;
    }
    private void OnRotate(bool _L0OrR1) { ProcessRotation(_L0OrR1); }

    private void OnJump() { NewJump(); }

    void Update()
    {
        if (!rotManager.m_rotate)
        {
            GroundRayCheck();
            if (m_animator.speed != 1) { m_animator.speed = 1; }
        }
        else { m_animator.speed = 0; }

        if (!b_isGrounded) { WallRayCheck(); }

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
        rotManager.Rotate(b_dirChosen, m_desiredRotation);

        OrientSelfUp();
    }

    private void OnLandingCustom()
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

    public void Move(float _velocityHorz)
    {
        if (!rotManager.m_rotate)
        {
            Vector3 v3_targetVel = new Vector2(_velocityHorz * 10f, m_rigidBody.velocity.y);

            if (b_horizL && _velocityHorz > 0 || b_horizR && _velocityHorz < 0)
            {
                m_rigidBody.velocity = Vector3.SmoothDamp(m_rigidBody.velocity, v3_targetVel, ref m_Velocity, m_faSmoothing);
            }

            else if (!b_horizL && !b_horizR) { m_rigidBody.velocity = Vector3.SmoothDamp(m_rigidBody.velocity, v3_targetVel, ref m_Velocity, m_faSmoothing); }

            if (_velocityHorz > 0 && m_faceRight) { FlipSpriteDirection(); }
            else if (_velocityHorz < 0 && !m_faceRight) { FlipSpriteDirection(); }
        }
    }

    //ToDo: Improve The New Jump  so that you can do the Mario-Esque Held Button Jump!
    private void NewJump()
    {
        if (m_JumpCount < 2)
        {
            float _JumpForce = GetJumpForceAtHeight();

            if (m_rigidBody.velocity.y < 0) { m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, 0); }

            if (m_JumpCount == 0) { m_rigidBody.AddForce(m_rigidBody.transform.up * _JumpForce * m_rigidBody.mass, ForceMode2D.Impulse); }
            else { m_rigidBody.AddForce(m_rigidBody.transform.up * _JumpForce * .5f * m_rigidBody.mass, ForceMode2D.Impulse); }

            m_JumpCount++;
            b_isGrounded = false;
        }
    }

    private float GetJumpForceAtHeight() { return Mathf.Sqrt(Mathf.Abs(m_rigidBody.gravityScale * Physics2D.gravity.y) * JumpHeight * 2.0f); }

    private void OrientSelfUp()
    {
        if (!m_StoreRotation)
        {
            m_PrevRot = transform.localRotation;
            m_StoreRotation = true;
        }

        if (!rotManager.m_rotate && b_SelfOrient)
        {
            m_curentTime += Time.fixedDeltaTime;

            if (m_curentTime > m_RotationDelay * m_DurationScalar)
            {
                transform.localRotation = m_QuatDirection;

                m_rigidBody.simulated = true;
                b_SelfOrient = false;
                m_StoreRotation = false;
                m_curentTime = 0.0f;
            }
            else
            {
                float t = m_curentTime / (m_RotationDelay * m_DurationScalar);

                t = t * t * t * (t * (6f * t - 15f) + 10f);

                m_QuatDirection = rotManager.transform.localRotation;

                if (rotManager.rotationId == 1 || rotManager.rotationId == 3) { m_QuatDirection = Quaternion.Inverse(m_QuatDirection); }

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
        + 0.1f, LayerMask.GetMask("Blocks")) || */ (Physics2D.Raycast(transform.position, Vector2.down,
        GetComponent<CapsuleCollider2D>().bounds.extents.y + .6f, LayerMask.GetMask("Blocks"))) && (m_rigidBody.velocity.normalized.y <= 0))
        {
            b_isGrounded = true;

            b_horizL = false;
            b_horizR = false;
            m_JumpCount = 0;
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

        SpriteRenderer MyImage = gameObject.GetComponent<SpriteRenderer>();
        MyImage.flipX = !MyImage.flipX;
    }

    public void ProcessRotation(bool b_direction)
    {
        if (rotManager.m_rotate == false && !b_CanRotateSingle)
        {
            // m_PrevRot = transform.localRotation;
            switch (rotManager.rotationId)
            {
                case 0:
                    if (b_direction)
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 180 + 90);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId = 3;
                    }

                    else
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 90);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId++;
                    }

                    break;
                case 1:
                    if (b_direction)
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 0);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId = 0;
                    }
                    else
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 180);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId++;
                    }
                    break;

                case 2:
                    if (b_direction)
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 90);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId--;
                    }

                    else
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 180 + 90);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId++;
                    }
                    break;

                case 3:
                    if (b_direction)
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 180);
                        b_dirChosen = true;
                        m_rigidBody.simulated = false;
                        rotManager.rotationId--;
                    }

                    else
                    {
                        m_desiredRotation = Quaternion.Euler(0, 0, 0);
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
        else if (b_CanRotateSingle)
        {
            if (b_direction) { for (int i = 0; i < RotAreaList.Count; i++) { RotAreaList[i].RotSelect(1); } }

            else { for (int i = 0; i < RotAreaList.Count; i++) { RotAreaList[i].RotSelect(0); } }
        }
    }

}
