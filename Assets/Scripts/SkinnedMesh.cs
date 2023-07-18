using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class SkinnedMesh : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXGrapgh;
    public float refreshRate;
    void Start()
    {
        StartCoroutine(UpdateVFXGraph());
    }

    IEnumerator UpdateVFXGraph()
    {
        while (gameObject.activeSelf)
        {
            Mesh m = new Mesh();
            skinnedMesh.BakeMesh(m);
            VFXGrapgh.SetMesh("Mesh", m);

            yield return new WaitForSeconds(refreshRate);
        }
    }

    
}
