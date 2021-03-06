using UnityEngine;

public class A_Projectile : MonoBehaviour
{
    //Variables
    Rigidbody2D m_rigidBody;
    Collider2D m_collider;

    public RotationManager m_rotationRef;

    [Range(1, 20)] [SerializeField] float f_speed = 1;

    public Collider2D Collider { get => m_collider; }

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
        if (m_rigidBody.velocity.magnitude <= 0) { m_rigidBody.velocity = m_rigidBody.transform.right * f_speed; }
        if (m_rotationRef != null) { if (m_rotationRef.isRotating) { m_rigidBody.velocity = new Vector3(0, 0, 0); } }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Vector2 targetForce = Vector3.Normalize(other.gameObject.transform.position - gameObject.transform.position);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(targetForce, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
        else { Destroy(gameObject); }
    }
}
