using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerProgression : MonoBehaviour
{
    [SerializeField] private GameObject controllerPlayer;
    [SerializeField] private TextMeshProUGUI textQuestUI;
    [SerializeField] private Vector3[] spawnPosition;
    [SerializeField] private GameObject[] stages;
    [SerializeField] private string[] textQuest;

    private Transform player;
    void Awake()
    {
        player = controllerPlayer.transform.GetChild(1);
        player.transform.position = spawnPosition[PlayerPrefs.GetInt("Progress")];
        Instantiate(controllerPlayer);

        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(false);
        }
        stages[PlayerPrefs.GetInt("Progress")].SetActive(true);

        textQuestUI.text = textQuest[PlayerPrefs.GetInt("Progress")];
    }
}
