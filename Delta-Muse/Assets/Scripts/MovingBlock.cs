using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingBlock : MonoBehaviour
{
    [Range(0.01f, 10)] public float f_speed;
    [Range(0.1f, 5)] public float f_distance = .5f;

    Rigidbody2D rb2_Body;

    ///My initial position
    Vector2 v2_inPos;
    Vector2 v2_Dir;

    //temp fix
    public Transform Player;

    PlayerController MyController;

    [Range(1, 8)] public float f_customStep = 1; //Time Not good enough 

    float t;
    private void Start()
    {
        v2_inPos = transform.position;
        v2_Dir = transform.up;
    }

    private void Awake()
    {
        rb2_Body = GetComponent<Rigidbody2D>();
        if (Player != null)
        {
            MyController = Player.GetComponent<PlayerController>();
        }
    }

    //temp
    Vector2 huh;
    private bool b_up;

    private void Update()
    {
        huh = (Vector2)transform.position - v2_inPos;

        if (huh.magnitude > f_distance - 0.5f && b_up)
        {
            b_up = false;
        }
        else if (huh.magnitude < .5f && !b_up)
        {
            b_up = true;
            v2_inPos = (Vector2)transform.position - huh;
        }
    }

    private void FixedUpdate()
    {
        if (MyController.b_DirChosen == false)
        {

            // t += Time.deltaTime;
            Debug.Log("isadding");
            if (b_up)
            {
                transform.position = Vector2.Lerp(transform.position, v2_inPos + v2_Dir * f_distance, Time.deltaTime / f_speed);
            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, v2_inPos, Time.deltaTime / f_speed);

            }
        }

        else
        {
            v2_Dir = gameObject.transform.up;
        }
    }
}
