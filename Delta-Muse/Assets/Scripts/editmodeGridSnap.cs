using UnityEngine;
[ExecuteInEditMode]
public class editmodeGridSnap : MonoBehaviour
{
    float snapvalue = 1;
    private void Update()
    {
        float snapInverse = 1 / snapvalue;
        float x, y;
        // if snapValue = .5, x = 1.45 -> snapInverse = 2 -> x*2 => 2.90 -> round 2.90 => 3 -> 3/2 => 1.5
        // so 1.45 to nearest .5 is 1.5

//Smooth movement of tiles easy placement!
        x = Mathf.Floor(transform.position.x);
        y = Mathf.Floor(transform.position.y);
        transform.position = new Vector3(x + .5f, y + .5f);
    }
}