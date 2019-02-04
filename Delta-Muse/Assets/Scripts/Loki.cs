using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//The Follower
public class Loki : MonoBehaviour
{
    [SerializeField] private Transform m_target;

    private void Awake()
    {
        m_target = FindObjectOfType<PlayerController>().gameObject.transform;
    }

    private void Start()
    {

    }

    private void Move(float vel, bool jump)
    {

    }

}