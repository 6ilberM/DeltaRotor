using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RotationArea : MonoBehaviour
{

    GameObject Player;

    [SerializeField] RotTarget[] SingleRotObjs;

    PlayerController m_pController;
    public bool m_canRot;

    private float[] currentTime;
    private float m_rDelay = .2f;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        m_pController = Player.GetComponent<PlayerController>();
        currentTime = new float[SingleRotObjs.Length];
    }

    private void Start()
    {
        if (Player == null)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
    }

    public void RotSelect(int _dir)
    {
        if (!m_canRot && m_pController.canrotsingle == true)
        {
            for (int i = 0; i < SingleRotObjs.Length; i++)
            {
                switch (_dir)
                {
                    case 0:
                    
                        break;
                    case 1:

                        break;
                    default:
                    
                        break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Player == other.gameObject)
        {
            m_pController.canrotsingle = true;
            // Debug.Log("dis be triggered");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (Player == other.gameObject)
        {
            m_pController.canrotsingle = false;
            // Debug.Log("wtf");
        }
    }
}