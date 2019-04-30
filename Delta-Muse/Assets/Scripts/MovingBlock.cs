using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingBlock : MonoBehaviour
{
    //This performs terribly and should not be used /:

    RotationManager m_myrotMgr;
    Rigidbody2D myrigidBody;
    BoxCollider2D myCollider;
    // bool b_groundHit, b_doOnce;

    private void Awake()
    {
        m_myrotMgr = transform.parent.GetComponent<RotationManager>();
        myrigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }


    private void Update()
    {
        myrigidBody.simulated = true;

        if (m_myrotMgr.m_rotate)
        {
            myrigidBody.simulated = false;
            myrigidBody.velocity = Vector3.zero;
            myrigidBody.bodyType = RigidbodyType2D.Dynamic;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.collider.gameObject.layer == gameObject.layer)
        {
            myrigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            // myrigidBody.velocity = Vector3.zero;
        }
    }

}