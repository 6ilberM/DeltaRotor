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
    GameObject myobj2;

    private void Update()
    {
         
        int numColliders = 1;
        //Colliders of the reflected Gameobjs
        Collider2D[] reflectedGO = new Collider2D[numColliders];
        // Set you filters here according to https://docs.unity3d.com/ScriptReference/ContactFilter2D.html
        int colliderCount = m_myCollider.OverlapCollider(co_projectileFilter, reflectedGO);
        for (int i = 0; i < colliderCount; i++)
        {

            // Do bounce angles next then PLEASE COMPLETE A LEVEL GOD DAMN HAHA...
            if (colliderTemp[i] != reflectedGO[i])
            {
                // ; Debug.Log(reflectedGO[i].gameObject.transform.rotation);

                Vector3 temp = reflectedGO[i].gameObject.GetComponent<Rigidbody2D>().velocity.normalized;

                Vector3 normalVector = PlaceHolder.position - gameObject.transform.position;


                temp = Vector3.Reflect(temp, normalVector);

                // var temp2 = Quaternion.LookRotation(temp, reflectedGO[i].gameObject.transform.root.up);
                Quaternion temp2 = Quaternion.LookRotation(temp, reflectedGO[i].gameObject.transform.root.up);


                if (myobj2 == null)
                {
                    myobj2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                }
                Debug.DrawRay(transform.position, temp2 * temp * 3, Color.red, .4f, true);

                // myobj2.transform.rotation = Quaternion.RotateTowards(myobj2.transform.rotation, temp2, 10.0f);


                myobj2.transform.position = temp;

                // reflectedGO[i].gameObject.transform.rotation = temp2;
                reflectedGO[i].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                colliderTemp = reflectedGO;

            }
            if (false == colliderTemp[i].IsTouching(m_myCollider))
            {
                colliderTemp[i] = null;
            }
        }

        if (colliderCount > 0)
        {
        }
    }

}