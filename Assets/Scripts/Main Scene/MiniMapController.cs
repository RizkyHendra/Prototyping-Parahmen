using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    private Transform target;
    private GameObject player;

    public GameObject GUI, mainMap;
    private bool isOpen;

    public float minX = -5.0f;
    public float maxX = 5.0f;
    public float minY = -5.0f;
    public float maxY = 5.0f;
    public GameObject questMarker;
    private Vector3 originalPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.GetComponent<Transform>();
        isOpen = false;

        originalPosition = PlayerProgression.Instance.currentStage.questSpawnPosition;
    }

    private void Update()
    {
        MapView();
        SetQuestMiniMap();
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
    }

    private void MapView()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(isOpen)
            {
                GUI.SetActive(true);
                mainMap.SetActive(false);
                isOpen = false;
            }
            else
            {
                GUI.SetActive(false);
                mainMap.SetActive(true);
                isOpen = true;
            }
        }
    }

    private void SetQuestMiniMap()
    {
        questMarker.transform.position = new Vector3(originalPosition.x, questMarker.transform.position.y, originalPosition.z);
    }
}
