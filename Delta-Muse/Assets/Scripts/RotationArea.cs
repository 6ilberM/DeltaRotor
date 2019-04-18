using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RotationArea : MonoBehaviour
{
    [SerializeField] GameObject[] SingleRotObjs;
    GameObject Player;
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

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

    private void FixedUpdate()
    {

    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (Player == other.gameObject && Player.GetComponent<PlayerController>().canrotsingle != true)
        {
            Player.GetComponent<PlayerController>().canrotsingle = true;
            Debug.Log("dis be triggered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Player == other.gameObject)
        {
            Player.GetComponent<PlayerController>().canrotsingle = false;
        }
    }
}