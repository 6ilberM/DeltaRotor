using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Spin : MonoBehaviour
{
    float dt = -Mathf.PI;
    void LateUpdate()
    {
        dt += Time.deltaTime;
        if (dt >= Mathf.PI) { dt = -Mathf.PI; }

        transform.Rotate(Vector3.forward, (Time.deltaTime * 50 * Mathf.Cos(3 * dt)));
    }
}
