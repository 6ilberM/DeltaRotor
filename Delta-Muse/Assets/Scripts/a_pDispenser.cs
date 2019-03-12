using UnityEngine;

public class a_pDispenser : MonoBehaviour
{
    [Range(.1f, 5)] [SerializeField] float m_Delay = 0.1f;
    bool inactive = false;

    public GameObject go_pref;
    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
    public RotationManager m_rotationRef;

    void Start()
    {
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", m_Delay, m_Delay);
    }
    public void changeActiveStatus()
    {
        inactive = !inactive;
        return;
    }

    void Spawn()
    {
        if (inactive || m_rotationRef.m_rotate)
        {
            // ... exit the function.
            return;
        }

        // Find a random index between zero and one less than the number of spawn points.
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject myobj = Instantiate(go_pref, spawnPoints[i].position, spawnPoints[i].rotation);
            myobj.transform.parent = transform.parent;
        }
    }

}