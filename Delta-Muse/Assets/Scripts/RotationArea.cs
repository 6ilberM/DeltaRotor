using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RotationArea : MonoBehaviour
{
    struct QuatOldDesired
    {
        public Quaternion OldRot;
        public Quaternion DesiredRot;
    }

    [SerializeField] GameObject[] SingleRotObjs;
    private QuatOldDesired[] a_QuatArry;

    GameObject Player;
    public bool m_canRot;
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        a_QuatArry = new QuatOldDesired[SingleRotObjs.Length];

    }
    private void Start()
    {
        if (Player == null)
        {
            gameObject.SetActive(false);
        }
    }
    bool b_rotRight, b_rotLeft;

    private void Update()
    {
        if (!m_canRot && Player.GetComponent<PlayerController>().canrotsingle == true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                for (int i = 0; i < SingleRotObjs.Length; i++)
                {
                    a_QuatArry[i].OldRot = SingleRotObjs[i].transform.rotation;

                    // a_QuatArry[i].DesiredRot = SingleRotObjs[i].transform.localRotation * Quaternion.Euler(0, 0, -90);
                }
                m_canRot = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                for (int i = 0; i < SingleRotObjs.Length; i++)
                {
                    a_QuatArry[i].OldRot = SingleRotObjs[i].transform.rotation;

                    // a_QuatArry[i].DesiredRot = SingleRotObjs[i].transform.rotation * Quaternion.Euler(0, 0, 90);
                }
                m_canRot = true;

            }
        }

    }

    private void FixedUpdate()
    {

        if (m_canRot)
        {
            for (int i = 0; i < SingleRotObjs.Length; i++)
            {
                // SingleRotObjs[i].transform.rotation = a_QuatArry[i].DesiredRot;
            }
            m_canRot = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Player == other.gameObject)
        {
            Player.GetComponent<PlayerController>().canrotsingle = true;
            Debug.Log("dis be triggered");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (Player == other.gameObject)
        {
            Player.GetComponent<PlayerController>().canrotsingle = false;
            Debug.Log("wtf");
        }
    }


}