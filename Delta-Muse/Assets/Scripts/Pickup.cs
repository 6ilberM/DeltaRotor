using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] [Range(0.75f, 1f)] private float f_stiffness = 0.95f;
    private Transform m_Target;
    private Rigidbody2D m_rb;
    private bool b_pickedUp;
    private float dt;
    [SerializeField] private float MaxSpeed = 4;

    [SerializeField] private bool pickupNew = true;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !b_pickedUp && this.enabled)
        {
            b_pickedUp = true;
            m_Target = other.transform;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            m_rb.velocity = Physics2D.gravity;
        }
    }

    private void LateUpdate()
    {
        if (b_pickedUp)
        {
            Vector3 _Dir = ((m_Target.position) - transform.position);
            Vector3 _DirOff = ((m_Target.position + Vector3.up * 1.5f) - transform.position);
            if (Mathf.Abs(_DirOff.magnitude) > 0.15f)
            {
                m_rb.velocity += Physics2D.gravity;
                if (!pickupNew)
                {
                    float f_speed = 4.0f;
                    if (_Dir.magnitude > 0.3f)
                    {
                        dt += Time.deltaTime;
                        transform.position = (_Dir.normalized * Time.deltaTime * f_speed) + transform.position;
                    }
                }
                else
                {
                    Vector2 desiredVel = _DirOff.normalized;
                    desiredVel *= Mathf.Lerp(_Dir.magnitude, MaxSpeed, Time.deltaTime * 0.8f);
                    Vector2 SteerForce = (desiredVel - m_rb.velocity) * f_stiffness;
                    m_rb.velocity += SteerForce;
                }
            }
            else { m_rb.velocity = Vector3.Lerp(m_rb.velocity, Vector3.zero, Time.deltaTime * 10f); }
        }
    }
}
