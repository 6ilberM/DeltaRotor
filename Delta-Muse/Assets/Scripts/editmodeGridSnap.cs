using UnityEngine;

[ExecuteInEditMode]
public class editmodeGridSnap : MonoBehaviour
{
    private void Update()
    {
        if (Application.isPlaying)
        {
        }
        else
        {
            float x, y;

            //Smooth movement of tiles easy placement!
            x = Mathf.Round(transform.position.x);
            y = Mathf.Round(transform.position.y);
            transform.position = new Vector3(x + .5f, y + .5f);
        }
    }

}