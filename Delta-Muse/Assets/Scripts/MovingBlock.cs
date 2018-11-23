using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingBlock : MonoBehaviour
{
    [Range(1, 8)] public int i_speed = 1;
    Rigidbody2D rb2_Body;
    //temp fix
    public Transform Player;
    PlayerController MyController;

    [Range(1, 8)] public float f_customStep = 1; //Time Not good enough 

    float t;
    private void Start()
    {

    }
    private void Awake()
    {
        rb2_Body = GetComponent<Rigidbody2D>();
        if (Player != null)
        {
            MyController = Player.GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        if (MyController.b_DirChosen == false)
        {
            t += Time.deltaTime;
            Debug.Log("isadding");
        }
    }


    private void FixedUpdate()
    {


        transform.position = SineFunction(transform.position.x, transform.position.y, t, f_customStep, i_speed);
    }
    const float pi = 3.14f;
    // static Vector3 SineFunction(float _X, float _z, float _t, float Amplitude)
    // {
    //     Vector3 p;
    //     p.x = _X;
    //     p.y = Mathf.Sin(pi * (_X + _t));
    //     p.z = _z;
    //     return p;
    // }
    Vector2 SineFunction(float _X, float _Y, float _t, float _period, float Amplitude)
    {
        Vector2 p;
        Quaternion why = gameObject.transform.rotation;
        Vector3 hm = why.eulerAngles;
        if (!MyController.b_DirChosen)
        {
            p.x = _X;
            p.y = Amplitude * Mathf.Sin((_X + (_t)) / _period);

        }
        else
        {
            p.x = _X;
            p.y = _Y;
        }
        return p;
    }

}