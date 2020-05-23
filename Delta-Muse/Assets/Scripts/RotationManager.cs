using UnityEngine;
using System.Collections;
using EasingHelper;

public class RotationManager : MonoBehaviour
{
    private Quaternion m_startRotation;
    private float m_dt;
    private PlayerController player;
    private EasingFunction.Function _easeFunc;

    public bool m_rotate, m_doOnce;
    [SerializeField] float m_rDelay = 0.39f;
    public int rotationId = 0;

    [Tooltip("Specifies the type of ease that this Rotating Object will have")]
    ///<summary>The type of ease</summary>
    public EasingFunction.Ease m_easeType = EasingFunction.Ease.EaseInOutQuad;

    private void Start()
    {
        player = Object.FindObjectOfType<PlayerController>();
        _easeFunc = EasingFunction.GetEasingFunction(m_easeType);
    }

    public void Rotate(Quaternion _desiredRotation)
    {
        if (!m_doOnce)
        {
            m_startRotation = transform.rotation;
            m_doOnce = true;
        }

        m_dt += Time.fixedDeltaTime;
        bool wasOrienting = player.b_SelfOrient;

        //Close Enough? w/ thresholdCheck
        if (m_dt >= m_rDelay)
        {
            transform.rotation = _desiredRotation;

            player.b_dirChosen = false;
            player.b_SelfOrient = true;
            m_rotate = false;

            if (wasOrienting == player.b_SelfOrient) { player.m_rotationDuration = 1.5f; }
            else { player.m_rotationDuration = 1; }

            m_dt = 0.0f;
            //Or you could set do once back off and it can once again go through
            m_startRotation = _desiredRotation;
            //how much force should be lost after Rotating 
            if (player.m_rigidBody.velocity.y <= -0.5f)
            {
                player.m_rigidBody.velocity = new Vector2(player.m_rigidBody.velocity.x, player.m_rigidBody.velocity.y * 0.25f);
            }
        }
        else
        {
            float t = (float)m_dt / (float)m_rDelay;

            float value = _easeFunc(0, 1, t);
            // easeoutquad
            // t = (t * (2 - t));

            transform.rotation = Quaternion.Slerp(m_startRotation, _desiredRotation, value);
        }
    }

    public IEnumerator RotateWhile(Quaternion _rot)
    {
        m_rotate = true;
        while (m_rotate)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            Rotate(_rot);
        }
    }

}
