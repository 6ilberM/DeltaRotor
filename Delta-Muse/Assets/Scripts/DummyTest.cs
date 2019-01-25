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
        if (!TestScript.m_rotate)
        {
            imperative = false;
        }
        if (TestScript != null && Input.GetKeyDown(KeyCode.Period) && !TestScript.m_rotate)
        {
            //execute
            imperative = true;
            TestScript.rotationId = id;
        }
    }
    bool imperative;
    private void FixedUpdate()
    {
        if (TestScript != null)
        {
            //execute
            TestScript.Rotate(imperative, id);

        }

    }
}