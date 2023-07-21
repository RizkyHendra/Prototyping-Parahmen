using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

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

    [SerializeField] private string[] textDialogueText;
    [SerializeField] private string[] nameDialogueText;
    [SerializeField] private Sprite[] characterImageSprite;

    [SerializeField] private float textSpeed;

    public PlayerInput input;
    public bool dialogueEnd;

    private int index;

    void Start()
    {
        textDialogue.text = string.Empty;
        nameDialogue.text = nameDialogueText[index];
        characterImage.sprite = characterImageSprite[index];
        dialogueEnd = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textDialogue.text == textDialogueText[index])
            {
                NextLine();
                nameDialogue.text = nameDialogueText[index];
                characterImage.sprite = characterImageSprite[index];
            }
            else
            {
                StopAllCoroutines();
                textDialogue.text = textDialogueText[index];  
            }
        }
    }

    public void StartDialogue()
    {
        index = 0;
        input.DeactivateInput();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in textDialogueText[index].ToCharArray())
        {
            textDialogue.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < textDialogueText.Length - 1)
        {
            index++;
            textDialogue.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            input.ActivateInput();
            dialogueEnd = true;
        }
    }
}
