using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RotationArea : MonoBehaviour
{
    GameObject Player;
    PlayerController m_pController;
#pragma warning disable 0649
    [SerializeField] RotTarget[] rotatingObjects;
#pragma warning restore 0649

    public bool m_canRot;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        m_pController = Player.GetComponent<PlayerController>();
    }

    private void Start()
    {
        if (Player == null)
        {
            gameObject.SetActive(false);
        }
    }

    public void RotSelect(int _dir)
    {
        switch (_dir)
        {
            case 0:
                for (int i = 0; i < rotatingObjects.Length; i++)
                {
                    rotatingObjects[i].RotarIzquierda();
                }
                break;
            case 1:
                for (int i = 0; i < rotatingObjects.Length; i++)
                {
                    rotatingObjects[i].RotarDerecha();
                }

                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Player == other.gameObject)
        {
            m_pController.b_CanRotateSingle = true;
            if (!m_pController.RotAreaList.Contains(this)) { m_pController.RotAreaList.Add(this); }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (Player == other.gameObject)
        {
            m_pController.b_CanRotateSingle = false;
            if (m_pController.RotAreaList.Contains(this)) { m_pController.RotAreaList.Remove(this); }
        }
    }
}
