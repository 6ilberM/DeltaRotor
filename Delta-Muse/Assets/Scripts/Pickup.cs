using Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickup : MonoBehaviour
{
    public DPortal ref_portal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Wow!");
        //Play Little animation here and sound Woo!
        //Start Timer!
        Destroy(gameObject);
        TimerToVanish();
    }


    IEnumerator TimerToVanish()
    {
        Destroy(gameObject);
        Debug.Log("shouldbedead");
        yield return new WaitForSeconds(1);

    }
}