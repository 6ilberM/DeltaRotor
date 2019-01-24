using Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickup : MonoBehaviour
{
    public DPortal[] ref_portal;

    private void OnTriggerEnter2D(Collider2D other)
    {
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

        //Play Little animation here and sound Woo!
        //Start Timer!
        // Destroy(gameObject);
        // TimerToVanish();
    }

    IEnumerator TimerToVanish()
    {
        Destroy(gameObject);
        Debug.Log("shouldbedead");
        yield return new WaitForSeconds(1);

    }
}