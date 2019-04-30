using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    private Rigidbody2D rb_Body;
    private float currentTime;
    PlayerController player;

    public bool m_rotate, m_doOnce;
    [SerializeField] float m_rDelay = 0.39f;
    public int rotationId = 0;

    Quaternion prev;

    // Use this for initialization

    void Start()
    {
        player = Object.FindObjectOfType<PlayerController>();
    }

    //Handles World Rotation
    public void Rotate(bool _dirChosen, Quaternion _desiredRotation)
    {
        if (!m_doOnce)
        {
            prev = transform.rotation;
            m_doOnce = true;
        }

        if (_dirChosen == true)
        {
            m_rotate = true;
            currentTime += Time.fixedDeltaTime;
            bool wasOrienting = player.b_ShouldSelfOrient;

            //Close Enough? w/ thresholdCheck
            if (currentTime >= m_rDelay)
            {
                transform.rotation = _desiredRotation;
                player.b_dirChosen = false;
                m_rotate = false;
                player.b_ShouldSelfOrient = true;

                if (wasOrienting == player.b_ShouldSelfOrient)
                {
                    player.DurationScalar = 1.5f;
                }
                else
                {
                    player.DurationScalar = 1;

                }
                currentTime = 0.0f;
                //Or you could set do once back off and it can once again go through
                prev = _desiredRotation;
                //how much force should be lost after Rotating 
                if (player.m_rigidBody.velocity.y <= -0.5f)
                {
                    player.m_rigidBody.velocity = new Vector2(player.m_rigidBody.velocity.x, player.m_rigidBody.velocity.y * 0.25f);
                }
            }
            else
            {
                float t = currentTime / m_rDelay;
                // easeout cubic
                // t = (1 + (--t) * t * t);
                // easeoutquart
                // t = (--t) * t;
                // t =( 1 - t * t);

                // easeoutquad
                t = (t * (2 - t));
                transform.rotation = Quaternion.Slerp(prev, _desiredRotation, t);
            }
        }
    }

    //Handles World Rotation
    public void Rotate(bool _confirm, int _ID)
    {
        if (_confirm == true)
        {
            m_rotate = true;
            currentTime += Time.deltaTime;
            player.m_rigidBody.simulated = false;
            Quaternion DesiredRotation = transform.rotation;

            switch (_ID)
            {
                case 0:
                    DesiredRotation = Quaternion.Euler(0, 0, 0);
                    break;

                case 1:
                    DesiredRotation = Quaternion.Euler(0, 0, 90);

                    break;
                case 2:
                    DesiredRotation = Quaternion.Euler(0, 0, 180);

                    break;
                case 3:
                    DesiredRotation = Quaternion.Euler(0, 0, 270);

                    break;
                default:
                    break;
            }

            //Close Enough? w/ thresholdCheck
            if (currentTime > m_rDelay)
            {
                transform.rotation = DesiredRotation;
                player.b_ShouldSelfOrient = true;
                m_rotate = false;
                currentTime = 0.0f;
                // // player.b_lock1 = false;
                // player.m_rigidBody.simulated = true;
                prev = DesiredRotation;
                //how much force should be lost after Rotating 
                if (player.m_rigidBody.velocity.y <= -0.5f)
                {
                    player.m_rigidBody.velocity = new Vector2(player.m_rigidBody.velocity.x, player.m_rigidBody.velocity.y * 0.25f);
                }
            }
            else
            {
                //percent of lerp
                float t = currentTime / m_rDelay;

                //easeOutQuad
                t = (t * (2 - t));
                transform.rotation = Quaternion.Slerp(transform.rotation, DesiredRotation, t);
            }
        }
    }
}
