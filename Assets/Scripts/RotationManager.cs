using UnityEngine;
using System.Collections;
using EasingHelper;

public class RotationManager : MonoBehaviour
{
    private Quaternion m_startRotation;
    private float m_dt;
    private PlayerController player;
    private EasingFunction.Function m_easeFunc;

    public bool isRotating, m_doOnce;
    [SerializeField] float m_rDelay = 0.39f;
    public int rotationId = 0;

    [Tooltip("Specifies the type of ease that this Rotating Object will have")]
    ///<summary>The type of ease</summary>
    public EasingFunction.Ease m_easeType = EasingFunction.Ease.EaseInOutQuad;

    private void Awake()
    {
        m_easeFunc = EasingFunction.GetEasingFunction(m_easeType);
    }

    private void Start()
    {
        player = Object.FindObjectOfType<PlayerController>();
    }

#if UNITY_EDITOR
    private void OnValidate() { if (Application.isPlaying && m_easeFunc != EasingFunction.GetEasingFunction(m_easeType)) { m_easeFunc = EasingFunction.GetEasingFunction(m_easeType); } }
#endif


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
            isRotating = false;

            if (wasOrienting == player.b_SelfOrient) { player.m_rotationDuration = 1.5f; }
            else { player.m_rotationDuration = 1; }

            m_dt = 0.0f;
            //Or you could set do once back off and it can once again go through
            m_startRotation = _desiredRotation;
            //how much force should be lost after Rotating 
            if (player.m_rb.velocity.y <= -0.5f) { player.m_rb.velocity = new Vector2(player.m_rb.velocity.x, player.m_rb.velocity.y * 0.25f); }
            m_doOnce = false;
        }
        else
        {
            float t = (float)m_dt / (float)m_rDelay;

            float value = m_easeFunc(0, 1, t);
            // easeoutquad
            // t = (t * (2 - t));

            transform.rotation = Quaternion.Slerp(m_startRotation, _desiredRotation, value);
        }
    }

    public IEnumerator RotateWhile(Quaternion _rot)
    {
        //Don't allow another Rotation if we're already undergoing one
        if (!isRotating)
        {
            isRotating = true;
            while (isRotating)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
                Rotate(_rot);
            }
        }
        else
        {
            Debug.Log("Rotation Already Undergoing");
            yield return default;
        }
    }

}
