using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingBlock : MonoBehaviour
{
    [Range(0.1f, 25)] public float f_DesiredTime = 1;
    [Range(0.1f, 5)] public float f_distance = .5f;

    float f_dt;

    //temp
    Vector2 huh;
    private bool b_up;

    ///My initial position
    Vector2 v2_inPos;
    Vector2 v2_Dir;

    //temp fix
    public Transform Player;

    PlayerController MyController;

    [Range(1, 8)] public float f_customStep = 1; //Time Not good enough 

    private void Start()
    {
        v2_inPos = transform.position;
        v2_Dir = transform.up;
    }

    private void Awake()
    {
        if (Player != null)
        {
            MyController = Player.GetComponent<PlayerController>();
        }
    }


    private void Update()
    {
        huh = (Vector2)transform.position - v2_inPos;

        if (huh.magnitude > f_distance - 0.5f && b_up)
        {
            b_up = false;
            f_dt = 0;
        }
        else if (huh.magnitude < .5f && !b_up)
        {
            b_up = true;
            v2_inPos = (Vector2)transform.position - huh;
            f_dt = 0;
        }
    }

    private void FixedUpdate()
    {
        if (MyController.b_DirChosen == false)
        {
            f_dt += Time.deltaTime;
            if (b_up)
            {
                transform.position = Vector2.Lerp(transform.position, v2_inPos + v2_Dir * f_distance, f_dt / f_DesiredTime);
            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, v2_inPos, f_dt / f_DesiredTime);
            }
        }
        else
        {
            v2_Dir = gameObject.transform.up;
        }
    }
}
