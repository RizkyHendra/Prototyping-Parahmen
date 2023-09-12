using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageTrail : MonoBehaviour
{
    public bool trarilActif = false;
    public float activeTime = 3f;
    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public Transform positionToSpawn;
    public float meshDestroyDelay = 3f;

    [Header("Shader Related")]
    public Material mat1;
    public Material mat2;
    public Material mat3;
    //public string shaderVarRef;
    //public float shaderVarRate = 0.1f;
    //public float shaderVarRefreshRate = 0.05f;

    private bool isTrailActive = false;
    private SkinnedMeshRenderer[] skinnedMeshRender;
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTrailActive)
        {
           
            StartCoroutine(ActiveTrail(activeTime));
        }
    }

    public void RageTrails()
    {
        trarilActif = true;
        if (trarilActif == true)
        {
            StartCoroutine(ActiveTrail(activeTime));
        }
       
    }

    IEnumerator ActiveTrail(float timeActive)
    {
       
            while (timeActive > 0)
            {
                timeActive -= meshRefreshRate;
                if (skinnedMeshRender == null)
                    skinnedMeshRender = GetComponentsInChildren<SkinnedMeshRenderer>();
                for (int i = 0; i < skinnedMeshRender.Length; i++)
                {
                    GameObject gObj = new GameObject();
                    gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                    MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                    MeshFilter mf = gObj.AddComponent<MeshFilter>();

                    Mesh mesh = new Mesh();
                    skinnedMeshRender[i].BakeMesh(mesh);

                    mf.mesh = mesh;
                    Material[] material = new Material[] { mat1, mat2, mat3 };
                    mr.materials = material;

                    //StartCoroutine(AnimateMaterialFloat(mat1,mat2,mat3, 0, shaderVarRate, shaderVarRefreshRate));
                    Destroy(gObj, meshDestroyDelay);
                    
                }

                yield return new WaitForSeconds(meshRefreshRate);
            }
            isTrailActive = false;
        trarilActif = false;


    }
    //IEnumerator AnimateMaterialFloat(Material mat1 ,Material mat2,Material mat3, float goal, float rate, float refreshRate)
    //{
    //    float valueToAnimate1 = mat1.GetFloat(shaderVarRef);
    //    float valueToAnimate2 = mat2.GetFloat(shaderVarRef);
    //    float valueToAnimate3 = mat3.GetFloat(shaderVarRef);
    //    while (valueToAnimate1 > goal && valueToAnimate2 > goal && valueToAnimate3 > goal)
    //    {
    //        valueToAnimate1 -= rate;
    //        mat1.SetFloat(shaderVarRef, valueToAnimate1);
    //        valueToAnimate2 -= rate;
    //        mat2.SetFloat(shaderVarRef, valueToAnimate2);
    //        valueToAnimate3 -= rate;
    //        mat3.SetFloat(shaderVarRef, valueToAnimate3);
    //        yield return new WaitForSeconds(refreshRate);
    //    }
        
        
    //}
}
