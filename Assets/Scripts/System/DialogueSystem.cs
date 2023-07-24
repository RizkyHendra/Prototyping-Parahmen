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
    [SerializeField] private GameObject dialogueContainer, GUIobj;

    [SerializeField] private Animator animFade;

    public ItemDialogue[] classItemsDialogue;

    [SerializeField] private float textSpeed;

    public PlayerInput input;
    public bool dialogueEnd;

    private int index;
    private int classIndex;

    void Start()
    {
        dialogueEnd = false;
    }

    void Update()
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

    public void StartDialogue(int number)
    {
        textDialogue.gameObject.SetActive(true);
        nameDialogue.gameObject.SetActive(true);
        characterImage.gameObject.SetActive(true);
        dialogueContainer.SetActive(true);
        GUIobj.SetActive(false);
        index = 0;
        classIndex = number;
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
            textDialogue.gameObject.SetActive(false);
            nameDialogue.gameObject.SetActive(false);
            characterImage.gameObject.SetActive(false);
            dialogueContainer.SetActive(false);
            input.ActivateInput();
            dialogueEnd = true;

            StartCoroutine(fadeScreen(1f));
        }
    }

    private IEnumerator fadeScreen(float time)
    {
        animFade.Play("fade");
        yield return new WaitForSecondsRealtime(time);
        SceneManager.LoadScene(1);
    }
}

[System.Serializable]
public class ItemDialogue
{
    public string[] textDialogueText;
    public string[] nameDialogueText;
    public Sprite[] characterImageSprite;
}
