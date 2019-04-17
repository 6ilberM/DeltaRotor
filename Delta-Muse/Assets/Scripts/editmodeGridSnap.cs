using UnityEngine;

[ExecuteInEditMode]
public class editmodeGridSnap : MonoBehaviour
{
    private void Update()
    {
        if (Application.isPlaying)
        {
            // code executed in play mode
        }
        else
        {
            float x, y;

            //Smooth movement of tiles easy placement!
            x = Mathf.Floor(transform.position.x);
            y = Mathf.Floor(transform.position.y);
            transform.position = new Vector3(x + .5f, y + .5f);
        }
    }

}