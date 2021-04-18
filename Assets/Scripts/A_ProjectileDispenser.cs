using UnityEngine;

public class A_ProjectileDispenser : MonoBehaviour
{
    ///Represents How long it will take to fire again
    [Range(.1f, 5)] [SerializeField] float deltaFire = 0.5f;

    ///The time Difference that it should take to begin the Process
    [Range(.1f, 10)] [SerializeField] float m_Delay = 0.0f;

    bool inactive = false;

    public GameObject go_pref;
    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
    public RotationManager m_rotationRef;
    bool LemmeOut;
    float DeltaTime;

    public void changeActiveStatus()
    {
        inactive = !inactive;
        return;
    }

    private void Update()
    {
        if (m_Delay > Time.time) { LemmeOut = true; }

        if (LemmeOut && !m_rotationRef.isRotating)
        {
            DeltaTime += Time.deltaTime;

            if (DeltaTime >= deltaFire)
            {
                Spawn();
                DeltaTime = 0;
            }
        }

    }
    void Spawn()
    {
        if (inactive || m_rotationRef.isRotating) { return; }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject myobj = Instantiate(go_pref, spawnPoints[i].position, spawnPoints[i].rotation);
            myobj.transform.parent = transform.parent;
        }
    }
}
