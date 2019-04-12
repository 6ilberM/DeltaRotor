using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]

public class a_ProjBounce : MonoBehaviour
{

    public enum BounceOrientation
    {
        uLeft, uRight, dLeft, dRight
    }

    //this actor will move some things
    //Overlap methods 
    ContactFilter2D co_projectileFilter;
    Collider2D[] overlapResults;

    BoxCollider2D m_myCollider;


    Transform PlaceHolder;

    public BounceOrientation SpriteFace;

    //What if i just define the normal

    private void Awake()
    {
        Vector3 normalVector;
        m_myCollider = gameObject.GetComponent<BoxCollider2D>();

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

    //Remembers which Objects have gone through recently so they remain unaffected until they pass the next object should pass this unto The actor rather than the other way around.
    Collider2D[] colliderTemp = new Collider2D[10];

    private void Update()
    {

        // int numColliders = 1;
        // //Colliders of the reflected Gameobjs
        // Collider2D[] reflectedGO = new Collider2D[numColliders];
        // // Set you filters here according to https://docs.unity3d.com/ScriptReference/ContactFilter2D.html
        // int colliderCount = m_myCollider.OverlapCollider(co_projectileFilter, reflectedGO);
        // for (int i = 0; i < colliderCount; i++)
        // {
        //     // Do bounce angles next then PLEASE COMPLETE A LEVEL GOD DAMN HAHA...
        //     if (colliderTemp[i] != reflectedGO[i])
        //     {
        //         Vector3 MyDist = (reflectedGO[i].transform.position - gameObject.transform.position);
        //         if (MyDist.magnitude <= 0.2f)
        //         {
        //             Vector3 vec1 = reflectedGO[i].gameObject.GetComponent<Rigidbody2D>().velocity;

        //             Vector3 normalVector = PlaceHolder.position - gameObject.transform.position;

        //             vec1 = Vector3.Reflect(vec1.normalized, normalVector);
        //             Debug.Log(vec1.magnitude);
        //             Debug.DrawRay(gameObject.transform.position, vec1 * 5, Color.red, 0.2f, true);
        //             float desiredAngle = Mathf.Round((Mathf.Atan2(vec1.y, vec1.x) * Mathf.Rad2Deg));

        //             Vector3 test = (Vector3)reflectedGO[i].gameObject.GetComponent<Rigidbody2D>().velocity + normalVector;

        //             reflectedGO[i].gameObject.transform.position = gameObject.transform.position;

        //             reflectedGO[i].gameObject.transform.rotation = Quaternion.Euler(0, 0, desiredAngle);

        //             reflectedGO[i].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        //             colliderTemp[i] = reflectedGO[i];
        //         }
        //     }
        // }
        // Debug.Log(colliderTemp.Length);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Vector3 MyDist = (other.transform.position - gameObject.transform.position);

        Vector3 vec1 = other.gameObject.GetComponent<Rigidbody2D>().velocity;

        Vector3 normalVector = PlaceHolder.position - gameObject.transform.position;

        vec1 = Vector3.Reflect(vec1.normalized, normalVector);
        Debug.Log(vec1.magnitude);
        Debug.DrawRay(gameObject.transform.position, vec1 * 5, Color.red, 0.2f, true);
        float desiredAngle = Mathf.Round((Mathf.Atan2(vec1.y, vec1.x) * Mathf.Rad2Deg));

        Vector3 test = (Vector3)other.gameObject.GetComponent<Rigidbody2D>().velocity + normalVector;

        other.gameObject.transform.position = gameObject.transform.position;

        other.gameObject.transform.rotation = Quaternion.Euler(0, 0, desiredAngle);

        other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("exit");


    }
}