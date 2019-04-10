using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]

public class a_ProjBounce : MonoBehaviour
{

    public enum BounceOrientation
    {
        UpL, UpR, DownL, DownR
    }

    //this actor will move some things
    //Overlap methods 
    ContactFilter2D projectileCfilter;
    Collider2D[] overlapResults;

    BoxCollider2D myCollider;

    public BounceOrientation Direction;

    private void Awake()
    {
        myCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        projectileCfilter = new ContactFilter2D();
        projectileCfilter.SetLayerMask(LayerMask.GetMask("Interactable"));
    }

    Collider2D[] colliderstemp = new Collider2D[10];
    private void Update()

    {
        int numColliders = 10;
        Collider2D[] colliders = new Collider2D[numColliders];
        // Set you filters here according to https://docs.unity3d.com/ScriptReference/ContactFilter2D.html
        int colliderCount = myCollider.OverlapCollider(projectileCfilter, colliders);
        for (int i = 0; i < colliderCount; i++)
        {

            // Do bounce angles next then PLEASE COMPLETE A LEVEL GOD DAMN HAHA...
            if (colliderstemp[i] != colliders[i])
            {
                Debug.Log(colliders[i].gameObject.transform.rotation);


                Vector3 holder = colliders[i].gameObject.transform.rotation.eulerAngles;
                float f_holder = holder.z;

                colliders[i].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                colliderstemp = colliders;

            }
            if (false == colliderstemp[i].IsTouching(myCollider))
            {
                colliderstemp[i] = null;
            }
        }

        if (colliderCount > 0)
        {
            Debug.Log(colliderCount);
        }
    }

}