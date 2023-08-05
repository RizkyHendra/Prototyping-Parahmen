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

    [SerializeField] private TextMeshProUGUI textDialogue;
    [SerializeField] private TextMeshProUGUI nameDialogue;
    [SerializeField] private Image characterImage;

    [SerializeField] private GameObject dialogueObj, GUIObj;

    [SerializeField] private Animator animFade;
    [SerializeField] private float textSpeed;
    [SerializeField] private PlayerInput input;
    public bool onDialogueScene;

    private int index;
    private int classIndex;
    private int loadSceneIndex;

    public ItemDialogue[] classItemsDialogue;

    void Start()
    {
        onDialogueScene = false;

        dialogueObj.SetActive(false);
        GUIObj.SetActive(true);
    }

    void Update()
    {
        if(onDialogueScene == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (textDialogue.text == classItemsDialogue[classIndex].textDialogueText[index])
                {
                    NextLine();
                    nameDialogue.text = classItemsDialogue[classIndex].nameDialogueText[index];
                    characterImage.sprite = classItemsDialogue[classIndex].characterImageSprite[index];
                }
                else
                {
                    StopAllCoroutines();
                    textDialogue.text = classItemsDialogue[classIndex].textDialogueText[index];
                }
            }
        }
    }

    public void StartDialogue(int number, int loadScene)
    {
        dialogueObj.SetActive(true);
        GUIObj.SetActive(false);
        onDialogueScene = true;
        index = 0;
        classIndex = number;
        loadSceneIndex = loadScene;
        textDialogue.text = string.Empty;
        nameDialogue.text = classItemsDialogue[number].nameDialogueText[index];
        characterImage.sprite = classItemsDialogue[number].characterImageSprite[index];

        input.DeactivateInput();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in classItemsDialogue[classIndex].textDialogueText[index].ToCharArray())
        {
            textDialogue.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < classItemsDialogue[classIndex].textDialogueText.Length - 1)
        {
            index++;
            textDialogue.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueObj.SetActive(false);
            GUIObj.SetActive(true);
            input.ActivateInput();
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

[System.Serializable]
public class ItemDialogue
{
    public string[] textDialogueText;
    public string[] nameDialogueText;
    public Sprite[] characterImageSprite;
}
