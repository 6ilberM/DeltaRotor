using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class DPortal : MonoBehaviour
{
    public bool b_isOpen = false;

    private void Start()
    {
        //Animate Door Getting locked
    }
    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        gameObject.layer = 9; // Interactable
    }

    private void Update()
    {
        if (b_isOpen)
        {
            //Player Can Exit Level
            //Animate Openning -- Shake Screen a little
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "pl1" && b_isOpen)
        {
            Debug.Log("Let's Go to the next Level!");
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

        }
    }
}