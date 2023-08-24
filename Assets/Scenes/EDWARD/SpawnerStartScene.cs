using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.HighDefinition;

public class SpawnerStartScene : MonoBehaviour
{
    [Header("Spawner Setting")]
    [SerializeField] Transform spawner;
    [SerializeField] Vector3 spawnerOffset;
    [SerializeField] float spawnTime;

    [Header("Reference Prefab")]
    //Phase 01
    [SerializeField] GameObject bridge01Prefab;
    [SerializeField] GameObject bridge02Prefab;
    //Phase 02
    [SerializeField] GameObject finalBridgePrefab;

    [Header("Bridge List & Layers")]
    [SerializeField] List<GameObject> bridges = new List<GameObject>();

    [Header("Main Camera Reference")]
    [SerializeField] Transform mainCamera;

    public int gamePhase;
    bool canSpawnNextBridge;
    int bridgeSpawned;

    //phase 01
    float cameraSpeed;
    Vector3 cameraPosition;

    //Ray cameraRay;
    //RaycastHit cameraRayInfo;
    //float rayDistance = 40f;
    //[SerializeField] LayerMask bridgeLayer;

    //phase 02
    Vector3 finalBridgeCamPosition;
    GameObject finalBridgeCam;
    public GameObject door;
    [Header("Camera Settings")]
    [Range(0.001f, 1f)]
    [SerializeField] float cameraDecelerationSpeed = 0.5f;
    
    bool spawnedLastBridge;


    //phase 3
    Vector3 phase3CamPosition;

    //get character
    public GameObject character, characterMC;

    //setting
    public GameObject CanvasMenuObj, settingCam, fadeScreen;

    [Header("Volume")]
    [SerializeField] GameObject volume;

    private void Start()
    {
        fadeScreen.SetActive(false);

        canSpawnNextBridge = true;

        cameraPosition = Vector3.zero;
        cameraSpeed = spawnerOffset.z / spawnTime;

        gamePhase = 1;

        //move character
        character.GetComponent<StarterAssets.StarterAssetsInputs>().MoveInput(new Vector2(0, 1));
        characterMC.GetComponent<MovementInput>().moveAxis = new Vector2(0, 1);
    }
    private void Update()
    {
        SpawnBridge();
        MoveCamera();
        DeleteBridge(); 
    }

    private void SpawnBridge()
    {
        if (gamePhase == 1)
        {
            if (canSpawnNextBridge)
            {
                canSpawnNextBridge = false;

                if (bridgeSpawned < 3)
                {
                    bridges.Add(Instantiate(bridge01Prefab, spawner));
                    bridgeSpawned++;
                }
                else
                {
                    bridges.Add(Instantiate(bridge02Prefab, spawner));
                    bridgeSpawned = 0;
                }

                foreach (GameObject bridge in bridges)
                {
                    bridge.transform.parent = null;
                }

                spawner.position = spawner.position + spawnerOffset;
                StartCoroutine(WaitForTime());
            }
        }
        else if (gamePhase == 2)
        {
            if (!spawnedLastBridge)
            {
                spawnedLastBridge = true;
                bridges.Add(Instantiate(finalBridgePrefab, spawner));
                foreach (GameObject bridge in bridges)
                {
                    bridge.transform.parent = null;
                }
                finalBridgeCamPosition = GameObject.Find("finalBridgeCamPosition").transform.position;
                finalBridgeCam = GameObject.Find("finalBridgeCamPosition");
                door = GameObject.Find("door_2");
                phase3CamPosition = GameObject.Find("phase3CamPosition").transform.position;
                settingCam = GameObject.Find("SettingCamera");
                
            }
        }
    }
    IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(spawnTime);
        canSpawnNextBridge = true;
    }

    private void MoveCamera()
    {
        if (gamePhase == 1)
        {
            cameraPosition.z = cameraSpeed * Time.deltaTime;
            mainCamera.transform.position = mainCamera.transform.position + cameraPosition;
        }
        else if (gamePhase == 2)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.position, finalBridgeCamPosition, cameraDecelerationSpeed * Time.deltaTime);
            if (finalBridgeCam.GetComponent<finalLorongStopCharacter>().stop != true)
            {
                character.GetComponent<StarterAssets.StarterAssetsInputs>().sprint = true;
                characterMC.GetComponent<MovementInput>().moveAxis = new Vector2(0, 3);
            }
            else
            {
                CanvasMenuObj.SetActive(true);
                character.GetComponent<StarterAssets.StarterAssetsInputs>().MoveInput(new Vector2(0, 0));
                character.GetComponent<StarterAssets.StarterAssetsInputs>().sprint = false;

                characterMC.GetComponent<MovementInput>().moveAxis = new Vector2(0, 0);
            }
        }
        else if (gamePhase == 3)
        {
            door.GetComponent<Animator>().SetBool("character_nearby", true);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.position, phase3CamPosition, cameraDecelerationSpeed * Time.deltaTime);
            StartCoroutine(DelayLoadScene());
        }
    }

    private IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(2);
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<Animator>().Play("fade in");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }


    private void DeleteBridge()
    {
        if (bridges.Count>=20)
        {
            Destroy(bridges[0]);
            bridges.RemoveAt(0);
        }

        //cameraRay.origin = mainCamera.transform.position;
        //cameraRay.direction = -mainCamera.transform.forward;

        //Debug.DrawLine(cameraRay.origin, cameraRay.origin + (cameraRay.direction * rayDistance), Color.red);

        //if (Physics.Raycast(cameraRay.origin, cameraRay.direction, out cameraRayInfo, rayDistance, bridgeLayer))
        //{
        //    if (bridges.Contains(cameraRayInfo.collider.gameObject))
        //    {
        //        if (bridges.IndexOf(cameraRayInfo.collider.gameObject)>0)
        //        {
        //            Destroy(bridges[0]);
        //            bridges.RemoveAt(0);
        //        }
        //    }
        //}
    }

    public void NextFase()
    {
        gamePhase++;
    }
    public void Settings()
    {

        //mainCamera.GetComponent<Animator>().SetBool("Setting", true);
        //gamePhase = 10;
        //mainCamera.transform.position = Vector3.Lerp(mainCamera.position, finalBridgeCam.transform.position, cameraDecelerationSpeed * Time.deltaTime);
        //mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.rotation, finalBridgeCam.transform.rotation, cameraDecelerationSpeed * Time.deltaTime);
        settingCam.GetComponent<Camera>().enabled = true;

        DepthOfField dof;
        volume.GetComponent<Volume>().profile.TryGet<DepthOfField>(out dof);
        dof.active = true;
    }
    public void SettingsOff()
    {
        settingCam.GetComponent<Camera>().enabled = false;
        DepthOfField dof;
        volume.GetComponent<Volume>().profile.TryGet<DepthOfField>(out dof);
        dof.active = false;
    }
}
