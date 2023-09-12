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

    [SerializeField] private GameObject popUpQuest, questName;
    [SerializeField] private TextMeshProUGUI popUpQuestText;


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
        StartCoroutine(FadeAnim("fade out"));
        StartCoroutine(PopUpQuest());

        SoundManager.Instance.PlayBGM("BGM - Exploration");
    }

    public IEnumerator FadeAnim(string name)
    {
        prefabsFade.SetActive(true);
        prefabsFade.GetComponent<Animator>().Play(name);
        yield return new WaitForSeconds(1f);
        prefabsFade.SetActive(false);
    }

    private IEnumerator PopUpQuest()
    {
        popUpQuest.SetActive(false);
        popUpQuestText.text = currentStage.PopUpQuestName;
        yield return new WaitForSeconds(3f);
        SoundManager.Instance.PlaySFX("SFX - Swoosh");
        Time.timeScale = 0.2f;
        popUpQuest.SetActive(true);
        popUpQuest.GetComponent<Animator>().Play("popupquest");
        popUpQuest.GetComponent<Animator>().speed = 1 * 5f;
        questName.GetComponent<Animator>().speed = 1 * 5f;

        yield return new WaitForSeconds(3f/5f);
        Time.timeScale = 1f;
        popUpQuest.SetActive(false);
        questName.GetComponent<Animator>().speed = 1;
    }
}
