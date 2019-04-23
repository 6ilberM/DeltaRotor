using UnityEngine;


public class RotTarget : MonoBehaviour
{

    bool b_shouldRot;
    Transform myTransform;
    Quaternion m_initRot, m_desiredRot;

    float deltaTime;
    private float totalTime = .2f;

    public void RotarDerecha()
    {
        if (!b_shouldRot)
        {

            m_initRot = gameObject.transform.rotation;

            m_desiredRot = gameObject.transform.rotation * Quaternion.Euler(0, 0, -90);
            b_shouldRot = true;
        }

    }

    public void RotarIzquierda()
    {
        if (!b_shouldRot)
        {
            m_initRot = gameObject.transform.rotation;

            m_desiredRot = gameObject.transform.rotation * Quaternion.Euler(0, 0, 90);
            b_shouldRot = true;
        }

    }

    private void FixedUpdate()
    {
        if (b_shouldRot)
        {
            deltaTime += Time.fixedDeltaTime;
            if (deltaTime >= totalTime)
            {
                transform.rotation = m_desiredRot;
                b_shouldRot = false;
                deltaTime = 0;
            }
            else
            {
                float t = deltaTime / totalTime;
                t = (t * (2 - t));
                transform.rotation = Quaternion.Slerp(m_initRot, m_desiredRot, t);
            }

        }
    }
}