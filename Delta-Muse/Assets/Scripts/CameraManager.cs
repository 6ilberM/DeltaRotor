using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{


    public Transform myplayer;
    Vector2 Myposition;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Myposition = transform.position;
        // transform.position = new Vector(Mathf.Lerp(Myposition.x, myplayer.position.x, 1));
        transform.position = new Vector3(Mathf.Lerp(Myposition.x, myplayer.position.x, 1), transform.position.y, transform.position.z);

    }

}
