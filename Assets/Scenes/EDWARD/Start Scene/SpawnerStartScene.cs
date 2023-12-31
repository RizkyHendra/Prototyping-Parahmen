using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.HighDefinition;

public class SpawnerStartScene : MonoBehaviour
{
    [Header("Better UI")]
    public AudioSource audioSource;
    public float maxValue;
    public float scaleFactor = 1.0f;
    public static float[] samples = new float[128];
    public Image vignette;

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

    public GameObject settingVirtualCamera, playCamera;

    [Header("Volume")]
    [SerializeField] GameObject volume;

    private void Start()
    {
        SoundManager.Instance.PlayBGM("BGM - MainMenu");
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
        //DeleteBridge(); 

        BetterUIMethod();
    }

    private void SpawnBridge()
    {
        if (gamePhase == 1)
        {
            if (canSpawnNextBridge)
            {
                canSpawnNextBridge = false;

                if (bridges.Count >= 10)
                {
                    bridges[0].transform.position = spawner.position;
                    GameObject temp = bridges[0];
                    bridges.RemoveAt(0);
                    bridges.Add(temp);
                }
                else
                {
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
        playCamera.SetActive(true);
        yield return new WaitForSeconds(2);
        fadeScreen.SetActive(true);
        
        fadeScreen.GetComponent<Animator>().Play("fade in");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
        SoundManager.Instance.StopBGM("BGM - MainMenu");
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
        //settingCam.GetComponent<Camera>().enabled = true;
        settingVirtualCamera.SetActive(true);

        DepthOfField dof;
        volume.GetComponent<Volume>().profile.TryGet<DepthOfField>(out dof);
        dof.active = true;
    }
    public void SettingsOff()
    {
        //settingCam.GetComponent<Camera>().enabled = false;
        settingVirtualCamera.SetActive(false);
        DepthOfField dof;
        volume.GetComponent<Volume>().profile.TryGet<DepthOfField>(out dof);
        dof.active = false;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void _OnClickSFX()
    {
        SoundManager.Instance.PlaySFX("SFX - Button");
    }

    private void BetterUIMethod()
    {
        if (audioSource != null)
        {
            audioSource.GetOutputData(samples, 0);

            float vals = 0.0f;

            for (int i = 0; i < 128; i++)
            {
                vals += Mathf.Abs(samples[i]);
            }
            vals /= 128.0f;

            float db = 1.0f + (vals * 10.0f);

            float convertedValue = (db - 1) / (maxValue - 1);

            int tes = (int)(convertedValue*10)+10;
            Debug.Log("int" + tes + "float " + convertedValue*10);
            vignette.color = new Color32(255,255,255,(byte)tes);
        }
    }
}
