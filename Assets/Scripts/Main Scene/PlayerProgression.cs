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
        textQuestUI.text = currentStage.questName;
    }
}
