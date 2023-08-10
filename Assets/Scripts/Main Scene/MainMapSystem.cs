using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMapSystem : MonoBehaviour
{
    [Header("References")]
    public RectTransform minimapPoint_1;
    public RectTransform minimapPoint_2;
    public Transform worldPoint_1;
    public Transform worldPoint_2;

    public float offset;

    [Header("Player")]
    public RectTransform playerMinimap;
    private Transform playerWorld;

    [Header("MapSwitch")]
    public Image mapSprite;
    public Sprite mapFloor1, mapFloor2;
    public GameObject mainMapObject;
    public GameObject[] GUIObject;

    private float minimapRatio;
    private bool isShowed;

    private void Awake()
    {
        CalculateMapRatio();
    }

    private void Start()
    {
        playerWorld = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        mainMapObject.SetActive(false);
        isShowed = false;
    }

    private void Update()
    {
        playerMinimap.anchoredPosition = minimapPoint_1.anchoredPosition + new Vector2((playerWorld.position.x - worldPoint_1.position.x) * minimapRatio,
                                         (playerWorld.position.z - worldPoint_1.position.z) * minimapRatio);

        // Mendapatkan nilai rotasi pada sumbu Y dari playerWorld
        float playerRotationY = (-1 * playerWorld.rotation.eulerAngles.y)+offset;

        // Mendapatkan rotasi saat ini dari playerMinimap
        Vector3 currentMinimapRotation = playerMinimap.rotation.eulerAngles;

        // Mengatur nilai rotasi Z pada playerMinimap berdasarkan rotasi Y dari playerWorld
        currentMinimapRotation.z = playerRotationY;

        // Menetapkan kembali rotasi pada playerMinimap
        playerMinimap.rotation = Quaternion.Euler(currentMinimapRotation);

        MapSwicth();
        Controller();
    }

    public void CalculateMapRatio()
    {
        //distance world ignoring Y axis
        Vector3 distanceWorldVector = worldPoint_1.position - worldPoint_2.position;
        distanceWorldVector.y = 0f;
        float distanceWorld = distanceWorldVector.magnitude;

        //distance minimap
        float distanceMinimap = Mathf.Sqrt(
            Mathf.Pow((minimapPoint_1.anchoredPosition.x - minimapPoint_2.anchoredPosition.x), 2) +
            Mathf.Pow((minimapPoint_1.anchoredPosition.y - minimapPoint_2.anchoredPosition.y), 2));

        minimapRatio = distanceMinimap / distanceWorld;
    }

    private void MapSwicth()
    {
        if (playerWorld.transform.position.y < 1.8f)
        {
            mapSprite.sprite = mapFloor1;
        }
        else
        {
            mapSprite.sprite = mapFloor2;
        }
    }

    private void Controller()
    {
        if(isShowed)
        {
            if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
            {
                mainMapObject.SetActive(false);
                isShowed = false;

                for (int i = 0; i < GUIObject.Length; i++)
                {
                    GUIObject[i].SetActive(true);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                mainMapObject.SetActive(true);
                isShowed = true;

                for (int i = 0; i < GUIObject.Length; i++)
                {
                    GUIObject[i].SetActive(false);
                }
            }
        } 
    }
}
