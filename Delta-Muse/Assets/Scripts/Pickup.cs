using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Transform m_Target;
    private bool b_pickedUp;
    private float dt;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            b_pickedUp = true;
            m_Target = other.transform;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (b_pickedUp)
        {
            Vector3 _Dir = (m_Target.position - transform.position);
            float f_speed = 4.0f;
            if (_Dir.magnitude > 0.3f)
            {
                dt += Time.deltaTime;
                transform.position = (_Dir.normalized * Time.fixedDeltaTime * f_speed) + transform.position;
            }
        }
    }
}
