using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    private Rigidbody2D rb_Body;
    private float currentTime;
    public PlayerController player;

    public float f_RotDuration = 2.0f;

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
            currentTime += Time.deltaTime;

            float a, b;
            a = DesiredRotation.eulerAngles.z;
            b = transform.rotation.eulerAngles.z;
            //Close Enough? w/ thresholdCheck
            if (Mathf.Abs(a - b) <= 0.4f)
            {
                // transform.rotation = DesiredRotation;
                player.b_DirChosen = false;
                currentTime = 0.0f;
                player.rb2_MyBody.simulated = true;

                //how much force should be lost after Rotating 
                if (player.rb2_MyBody.velocity.y <= -0.5f)
                {
                    player.rb2_MyBody.velocity = new Vector2(player.rb2_MyBody.velocity.x, player.rb2_MyBody.velocity.y * 0.25f);
                }
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, DesiredRotation, currentTime / f_RotDuration);
            }
        }
    }
}
