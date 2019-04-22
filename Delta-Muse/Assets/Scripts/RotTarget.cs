using UnityEngine;


public class RotTarget : MonoBehaviour
{

    bool b_shouldRot;
    Transform myTransform;
    float deltaTime;
    private float totalTime = .2f;

    public bool B_shouldRot

    {
        get
        {
            return b_shouldRot;
        }

        set
        {
            b_shouldRot = value;
        }
    }

    private void Awake()
    {

    }
    private void Start()
    {
        myTransform = gameObject.transform;
    }

    private void Update()
    {
        // if ()
        // {
        //     for (int i = 0; i < SingleRotObjs.Length; i++)
        //     {
        //         a_QuatArry[i].OldRot = SingleRotObjs[i].transform.rotation;

        //         a_QuatArry[i].DesiredRot = SingleRotObjs[i].transform.rotation * Quaternion.Euler(0, 0, 90);
        //     }
        //     m_canRot = true;
        // }

        // if ()
        // {
        //     for (int i = 0; i < SingleRotObjs.Length; i++)
        //     {
        //         a_QuatArry[i].OldRot = SingleRotObjs[i].transform.rotation;

        //         a_QuatArry[i].DesiredRot = SingleRotObjs[i].transform.rotation * Quaternion.Euler(0, 0, -90);
        //     }
        //     m_canRot = true;

        // }

    }
    
    private void FixedUpdate()
    {
        // if (m_canRot)
        // {
        //     for (int i = 0; i < SingleRotObjs.Length; i++)
        //     {
        //         // SingleRotObjs[i].transform.rotation = a_QuatArry[i].DesiredRot;
        //         currentTime[i] += Time.fixedDeltaTime;

        //         //Close Enough? w/ thresholdCheck
        //         if (currentTime[i] >= m_rDelay && i == currentTime.Length)
        //         {

        //             SingleRotObjs[i].transform.rotation = a_QuatArry[i].DesiredRot;
        //             currentTime[i] = 0.0f;

        //             m_canRot = false;
        //             Debug.Log(i == currentTime.Length);
        //         }
        //         else
        //         {
        //             float t = currentTime[i] / m_rDelay;

        //             t = (t * (2 - t));
        //             SingleRotObjs[i].transform.rotation = Quaternion.Slerp(a_QuatArry[i].OldRot, a_QuatArry[i].DesiredRot, t);
        //         }
        //     }
        // }

    }
}