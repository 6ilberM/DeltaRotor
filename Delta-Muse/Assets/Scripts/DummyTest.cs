using UnityEngine;

public class DummyTest : MonoBehaviour
{
    public RotationManager TestScript;
    public int id;
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Plus))
        {
            id++;
            if (id >= 4)
            {
                id = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            id--;
            if (id <= -1)
            {
                id = 3;
            }
        }
    }

    private void FixedUpdate()
    {
        if (TestScript != null && Input.GetKey(KeyCode.Period))
        {
            //execute
            TestScript.Rotate(true, id);
        }

    }
}