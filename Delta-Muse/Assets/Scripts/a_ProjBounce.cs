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
        projectileCfilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    private void Update()

    {
        int numColliders = 10;
        Collider2D[] colliders = new Collider2D[numColliders];
        // Set you filters here according to https://docs.unity3d.com/ScriptReference/ContactFilter2D.html
        int colliderCount = myCollider.OverlapCollider(projectileCfilter, colliders);
        for (int i = 0; i < colliderCount; i++)
        {
//continue here
// Do bounce angles next then PLEASE COMPLETE A LEVEL GOD DAMN HAHA...

        }

        if (colliderCount > 0)
        {
            Debug.Log(colliderCount);
        }
    }

    private void LateUpdate()
    {

    }
}