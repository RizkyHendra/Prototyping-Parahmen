using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerProgression : MonoBehaviour
{
    public static PlayerProgression Instance;

    [SerializeField] private GameObject controllerPlayer;
    [SerializeField] private TextMeshProUGUI textQuestUI;

    [SerializeField] private StagesObject[] stagesObject;

    [SerializeField] private GameObject canvasUI;
    [SerializeField] private GameObject prefabsFade;


    public StagesObject currentStage;

    private Transform player;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentStage = stagesObject[PlayerPrefs.GetInt("Progress")];
        player = controllerPlayer.transform.GetChild(1);
        
        Instantiate(controllerPlayer);
        Instantiate(currentStage.prefabStage, currentStage.questSpawnPosition, currentStage.questSpawnRotation);

        player.transform.position = currentStage.playerSpawnPosition;
        player.transform.rotation = currentStage.playerSpawnRotation;
        textQuestUI.text = currentStage.questName;    
    }

    private void Start()
    {
        FadeAnim("fade out");
    }

    public void FadeAnim(string name)
    {
        GameObject spawnedPrefab = Instantiate(prefabsFade, transform.position, Quaternion.identity);
        spawnedPrefab.GetComponent<Animator>().Play(name);
        spawnedPrefab.transform.parent = canvasUI.transform; // Set the spawned prefab as child of the current GameObject

        Destroy(spawnedPrefab, 1.1f); // Destroy the spawned prefab after 1 second
    }
}
