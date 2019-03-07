using Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickup : MonoBehaviour
{
    public DPortal[] ref_portal;
    Transform m_TarTform;
    public bool m_pickedUp;


    private void OnTriggerEnter2D(Collider2D other)
    {
        m_pickedUp = true;
        // Debug.Log(other.name);
        if (ref_portal != null && ref_portal.Length > 1)
        {
            for (int i = 0; i < ref_portal.Length; i++)
            {
                ref_portal[i].b_isOpen = true;
            }
        }
        else
        {
            Debug.LogWarning("Dportal Ref,Cannot be found.");
        }
        m_TarTform = other.transform;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        //Play Little animation here and sound Woo!
        //Start Timer!
        // Destroy(gameObject);
        // TimerToVanish();
    }
    float dt;
    private void Update()
    {
        if (m_pickedUp)
        {
            Vector3 temp = (m_TarTform.position - transform.position);

            float f_speed = 4.0f;
            if (m_pickedUp && temp.magnitude > 0.3f)
            {
                dt += Time.deltaTime;

                transform.position = temp.normalized * Time.deltaTime * f_speed + transform.position;
            }
        }
    }



    IEnumerator TimerToVanish()
    {
        Destroy(gameObject);
        Debug.Log("shouldbedead");
        yield return new WaitForSeconds(1);

    }
}