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

    [SerializeField] private TextMeshProUGUI[] textDialogue;
    [SerializeField] private TextMeshProUGUI[] nameDialogue;
    [SerializeField] private Image[] characterImage;

    [SerializeField] private GameObject GUIObj, dialogueBorderObj, dialogueCamera;
    [SerializeField] private GameObject[] dialoguePanel;

    [SerializeField] private Animator animFade;
    [SerializeField] private float textSpeed;
    public GameObject player;
    public GameObject stuntMan;
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
        stuntMan.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");
        onDialogueScene = false;

        for (int i = 0; i < dialoguePanel.Length; i++)
        {
            dialoguePanel[i].SetActive(false);
        }
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
                if (textDialogue[0].text == currentStage.textDialogue[index])
                {
                    NextLine();

                    if (onDialogueScene)
                    {
                        if (currentStage.dialogueType[index] == 0)
                        {
                            StartCoroutine(delayActiveDialogueLeft(0.1f));
                        }
                        else if (currentStage.dialogueType[index] == 1)
                        {
                            StartCoroutine(delayActiveDialogueRight(0.1f));
                        }
                    }

                    nameDialogue[0].text = currentStage.nameDialogue[index];
                    nameDialogue[1].text = currentStage.nameDialogue[index];
                    characterImage[0].sprite = currentStage.character[index];
                    characterImage[1].sprite = currentStage.character[index];
                    dialogueCamera.transform.SetLocalPositionAndRotation(currentStage.cameraPosition[index], currentStage.cameraRotation[index]);
                }
                else
                {
                    StopAllCoroutines();
                    textDialogue[0].text = currentStage.textDialogue[index];
                    textDialogue[1].text = currentStage.textDialogue[index];
                }
            }

            stuntMan.SetActive(true);
            stuntMan.transform.position = currentStage.playerDialoguePosition;
            stuntMan.transform.rotation = currentStage.playerDialogueRotatation;
        }
    }

    public void StartDialogue(int loadScene)
    {
        StartCoroutine(PlayerProgression.Instance.FadeAnim("fade out"));

        dialogueBorderObj.SetActive(true);
        dialogueCamera.SetActive(true);
        GUIObj.SetActive(false);
        onDialogueScene = true;

        index = 0;
        loadSceneIndex = loadScene;

        if(onDialogueScene)
        {
            if (currentStage.dialogueType[index] == 0)
            {
                StartCoroutine(delayActiveDialogueLeft(0.1f));
            }
            else if (currentStage.dialogueType[index] == 1)
            {
                StartCoroutine(delayActiveDialogueRight(0.1f));
            }
        }

        textDialogue[0].text = string.Empty;
        textDialogue[1].text = string.Empty;

        nameDialogue[0].text = currentStage.nameDialogue[index];
        nameDialogue[1].text = currentStage.nameDialogue[index];

        characterImage[0].sprite = currentStage.character[index];
        characterImage[1].sprite = currentStage.character[index];

        dialogueCamera.transform.SetLocalPositionAndRotation(currentStage.cameraPosition[index], currentStage.cameraRotation[index]);

        player.GetComponent<PlayerInput>().DeactivateInput();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in currentStage.textDialogue[index].ToCharArray())
        {
            textDialogue[0].text += c;
            textDialogue[1].text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < currentStage.textDialogue.Length -1)
        {
            index++;
            Debug.Log(index);
            textDialogue[0].text = string.Empty;
            textDialogue[1].text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            for (int i = 0; i < dialoguePanel.Length; i++)
            {
                dialoguePanel[i].SetActive(false);
            }
            dialogueBorderObj.SetActive(false);
            dialogueCamera.SetActive(false);
            GUIObj.SetActive(false);
            //player.GetComponent<PlayerInput>().ActivateInput();
            onDialogueScene = false;
            stuntMan.SetActive(false);

            StartCoroutine(fadeScreen(1f, loadSceneIndex));
        }
    }

    private IEnumerator fadeScreen(float time, int sceneIndex)
    {
        animFade.Play("fade");
        yield return new WaitForSecondsRealtime(time);
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator delayActiveDialogueLeft(float time)
    {
        dialoguePanel[0].SetActive(false);
        dialoguePanel[1].SetActive(false);
        yield return new WaitForSecondsRealtime(time);
        dialoguePanel[0].SetActive(true);
        dialoguePanel[0].GetComponent<Animator>().Play("dialogue left");
        dialoguePanel[1].GetComponent<Animator>().Play("dialogue right");
    }

    private IEnumerator delayActiveDialogueRight(float time)
    {
        dialoguePanel[0].SetActive(false);
        dialoguePanel[1].SetActive(false);
        yield return new WaitForSecondsRealtime(time);
        dialoguePanel[1].SetActive(true);
        dialoguePanel[0].GetComponent<Animator>().Play("dialogue left");
        dialoguePanel[1].GetComponent<Animator>().Play("dialogue right");
    }
}
