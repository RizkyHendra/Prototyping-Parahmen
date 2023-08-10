using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    [SerializeField] private TextMeshProUGUI textDialogue;
    [SerializeField] private TextMeshProUGUI nameDialogue;
    [SerializeField] private Image characterImage;

    [SerializeField] private GameObject dialogueObj, GUIObj, dialogueBorderObj, dialogueCamera;

    [SerializeField] private Animator animFade;
    [SerializeField] private float textSpeed;
    private GameObject player;
    public bool onDialogueScene;

    private int index;
    private int loadSceneIndex;
    private StagesObject currentStage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentStage = PlayerProgression.Instance.currentStage;

        player = GameObject.FindGameObjectWithTag("Player");
        onDialogueScene = false;

        dialogueObj.SetActive(false);
        dialogueBorderObj.SetActive(false);
        dialogueCamera.SetActive(false);
        GUIObj.SetActive(true);
    }

    void Update()
    {
        if(onDialogueScene == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (textDialogue.text == currentStage.textDialogue[index])
                {
                    NextLine();
                    nameDialogue.text = currentStage.nameDialogue[index];
                    characterImage.sprite = currentStage.character[index];
                    dialogueCamera.transform.SetLocalPositionAndRotation(currentStage.cameraPosition[index], currentStage.cameraRotation[index]);
                }
                else
                {
                    StopAllCoroutines();
                    textDialogue.text = currentStage.textDialogue[index];
                }
            }
        }
    }

    public void StartDialogue(int loadScene)
    {
        dialogueObj.SetActive(true);
        dialogueBorderObj.SetActive(true);
        dialogueCamera.SetActive(true);
        GUIObj.SetActive(false);
        onDialogueScene = true;

        index = 0;
        loadSceneIndex = loadScene;
        textDialogue.text = string.Empty;

        nameDialogue.text = currentStage.nameDialogue[index];
        characterImage.sprite = currentStage.character[index];
        dialogueCamera.transform.SetLocalPositionAndRotation(currentStage.cameraPosition[index], currentStage.cameraRotation[index]);

        player.GetComponent<PlayerInput>().DeactivateInput();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in currentStage.textDialogue[index].ToCharArray())
        {
            textDialogue.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < currentStage.textDialogue.Length - 1)
        {
            index++;
            Debug.Log("tes");
            textDialogue.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueObj.SetActive(false);
            dialogueBorderObj.SetActive(false);
            dialogueCamera.SetActive(false);
            GUIObj.SetActive(true);
            player.GetComponent<PlayerInput>().ActivateInput();
            onDialogueScene = false;

            StartCoroutine(fadeScreen(1f, loadSceneIndex));
        }
    }

    private IEnumerator fadeScreen(float time, int sceneIndex)
    {
        animFade.Play("fade");
        yield return new WaitForSecondsRealtime(time);
        SceneManager.LoadScene(sceneIndex);
    }
}
