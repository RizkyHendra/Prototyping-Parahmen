using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private int dialogueIndex;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            DialogueSystem.Instance.StartDialogue(dialogueIndex, 4);
            Destroy(gameObject);
        }
    }
}
