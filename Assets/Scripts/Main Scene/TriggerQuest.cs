using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerQuest : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            DialogueSystem.Instance.StartDialogue(4);
        }
    }
}
