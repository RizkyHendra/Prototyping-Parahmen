using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTrigger : MonoBehaviour
{
    public GameObject dialogueObj;

    private void Start()
    {
        dialogueObj.SetActive(false);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            dialogueObj.SetActive(true);
            DialogueSystem.Instance.StartDialogue();
            Destroy(gameObject);
        }
    }
}
