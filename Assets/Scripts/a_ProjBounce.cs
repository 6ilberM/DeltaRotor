using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]

public class a_ProjBounce : MonoBehaviour
{

    public enum BounceOrientation
    {
        uLeft, uRight, dLeft, dRight
    }

    ContactFilter2D co_projectileFilter;
    Collider2D[] overlapResults;

    BoxCollider2D m_Collider;

    Transform PlaceHolder;

    public BounceOrientation SpriteFace;

    //Accesor
    public BoxCollider2D Collider { get { return m_Collider; } set { m_Collider = value; } }

    //What if i just define the normal

    private void Awake()
    {
        Vector3 normalVector;
        Collider = gameObject.GetComponent<BoxCollider2D>();

        switch (SpriteFace)
        {
            case BounceOrientation.uRight:
                normalVector = Quaternion.AngleAxis(45.0f, Vector3.forward) * gameObject.transform.parent.right;

                break;

            case BounceOrientation.uLeft:
                normalVector = Quaternion.AngleAxis(135.0f, Vector3.forward) * gameObject.transform.parent.right;

                break;

            case BounceOrientation.dLeft:
                normalVector = Quaternion.AngleAxis(225.0f, Vector3.forward) * gameObject.transform.parent.right;

                break;

            case BounceOrientation.dRight:
                normalVector = Quaternion.AngleAxis(315.0f, Vector3.forward) * gameObject.transform.parent.right;

                break;

            default:
                normalVector = new Vector3();
                break;
        }
        PlaceHolder = new GameObject().transform;
        PlaceHolder.name = "bNormalVec";
        PlaceHolder.position = transform.position;
        PlaceHolder.position += normalVector.normalized;
        PlaceHolder.SetParent(gameObject.transform);

    }

    private void Start()
    {
        co_projectileFilter = new ContactFilter2D();
        co_projectileFilter.SetLayerMask(LayerMask.GetMask("Interactable"));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<A_Projectile>() != null)
        {
            // Vector3 MyDist = (other.transform.position - gameObject.transform.position);

            Vector3 vec1 = other.gameObject.GetComponent<Rigidbody2D>().velocity;

            Vector3 normalVector = PlaceHolder.position - gameObject.transform.position;

            Vector3 v3temp = (vec1.normalized + normalVector);

            float f_magnitude = v3temp.magnitude;

            if (f_magnitude < 1)
            {
                vec1 = Vector3.Reflect(vec1.normalized, normalVector);

                float desiredAngle = Mathf.Round((Mathf.Atan2(vec1.y, vec1.x) * Mathf.Rad2Deg));

                other.gameObject.transform.position = gameObject.transform.position;

                other.gameObject.transform.rotation = Quaternion.Euler(0, 0, desiredAngle);

                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            }
            else
            {
                Destroy(other.gameObject);
                Debug.Log("else triggered");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("exit");
    }
}