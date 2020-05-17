using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_spin : MonoBehaviour
{
    float dt = -Mathf.PI;
    // Update is called once per frame
    void LateUpdate()
    {
        dt += Time.deltaTime;
        if (dt >= Mathf.PI)
        {
            dt = -Mathf.PI;
        }

        transform.Rotate(Vector3.forward, (Time.deltaTime * 50 * Mathf.Cos(3 * dt)));
    }
}
