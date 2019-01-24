using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    private Rigidbody2D rb_Body;
    private float currentTime;
    public PlayerController player;

    public bool b_Rotate;
    public float f_RotDuration = 2.0f;
    public int rotationId = 0;


    // Use this for initialization

    void Start()
    {
        player = Object.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Handles World Rotation
    public void Rotate(bool b_DirCh, Quaternion DesiredRotation)
    {
        if (b_DirCh == true)
        {
            b_Rotate = b_DirCh;
            currentTime += Time.fixedDeltaTime;

            float a, b;
            a = DesiredRotation.eulerAngles.z;
            b = transform.rotation.eulerAngles.z;
            //Close Enough? w/ thresholdCheck
            if (Mathf.Abs(a - b) <= 0.4f)
            {
                // transform.rotation = DesiredRotation;
                player.b_dirChosen = false;
                b_Rotate = b_DirCh;
                currentTime = 0.0f;
                player.m_rigidBody.simulated = true;

                //how much force should be lost after Rotating 
                if (player.m_rigidBody.velocity.y <= -0.5f)
                {
                    player.m_rigidBody.velocity = new Vector2(player.m_rigidBody.velocity.x, player.m_rigidBody.velocity.y * 0.25f);
                }
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, DesiredRotation, currentTime / f_RotDuration);
            }
        }
    }
}
