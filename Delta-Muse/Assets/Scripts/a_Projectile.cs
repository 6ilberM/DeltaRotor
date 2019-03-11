using UnityEngine;

public class a_Projectile : MonoBehaviour
{
    //Variables
    Rigidbody2D m_rigidBody;
    Collider2D m_collider;
    [Range(1, 20)] [SerializeField] float f_speed = 1;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        m_collider = gameObject.GetComponent<Collider2D>();
        m_rigidBody = gameObject.GetComponent<Rigidbody2D>();
        m_rigidBody.gravityScale = 0;
    }

    private void Start()
    {
        m_rigidBody.velocity = Vector2.right * f_speed;

    }
    private void Update()
    {

    }

}