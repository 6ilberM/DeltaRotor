using UnityEngine;

public class PropertyRandomSeed : MonoBehaviour
{
    private MaterialPropertyBlock mpb;

    [Range(43758.3453f, 43758.5454f)] public float seed = 43758.5453f;
    private float prevVal = 0;
    static readonly int shPropSeed = Shader.PropertyToID("_Seed");

    public MaterialPropertyBlock GetMatProperty
    {
        get
        {
            if (ReferenceEquals(mpb, null))
            {
                mpb = new MaterialPropertyBlock();
            }
            return mpb;
        }
    }

    private void OnEnable() { prevVal = seed; }

    void Update()
    {
        if (prevVal != seed)
        {
            prevVal = seed;
            var rend = GetComponent<Renderer>();
            rend.material.SetFloat(shPropSeed, seed);
        }
    }
}
