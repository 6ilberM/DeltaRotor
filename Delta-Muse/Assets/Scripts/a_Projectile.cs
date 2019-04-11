using UnityEngine;

public class a_Projectile : MonoBehaviour
{
    //Variables
    Rigidbody2D m_rigidBody;
    Collider2D m_collider;

    public RotationManager m_rotationRef;

    [Range(1, 20)] [SerializeField] float f_speed = 1;


    void Awake()
    {
        m_collider = gameObject.GetComponent<Collider2D>();
        m_rigidBody = gameObject.GetComponent<Rigidbody2D>();
        m_rigidBody.gravityScale = 0;
    }

    private void Start()
    {
        m_rigidBody.velocity = m_rigidBody.transform.right * f_speed;
        m_rotationRef = transform.parent.GetComponent<RotationManager>();

    }

    private void Update()
    {
        if (m_rigidBody.velocity.magnitude <= 0)
        {
            m_rigidBody.velocity = m_rigidBody.transform.right * f_speed;

        }

        if (m_rotationRef != null)
        {
            if (m_rotationRef.m_rotate)
            {
                m_rigidBody.velocity = new Vector3(0, 0, 0);
            }

        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }

}