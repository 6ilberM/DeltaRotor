using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{


    public Transform myplayer;
    Vector2 Myposition;
    private void FixedUpdate()
    {
        Myposition = transform.position;
        if (myplayer != null)
        {
            transform.position = new Vector3(
           Mathf.Lerp(Myposition.x, myplayer.position.x, .5f),
           Mathf.Lerp(Myposition.y, myplayer.position.y, .5f), transform.position.z);
        }
    }

}
