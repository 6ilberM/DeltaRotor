using UnityEngine;

public class a_Projectile : MonoBehaviour
{
    //Variables
    Rigidbody2D m_rigidBody;
     Collider2D m_collider;

    public RotationManager m_rotationRef;

    [Range(1, 20)] [SerializeField] float f_speed = 1;

    public Collider2D Collider
    {
        get
        {
            return m_collider;
        }

        set
        {
            m_collider = value;
        }
    }

    void Awake()
    {
        Collider = gameObject.GetComponent<Collider2D>();
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
        if (other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(other.gameObject.transform.position - gameObject.transform.position) * 150, ForceMode2D.Impulse);
            Destroy(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

}