using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerQuest : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            StartCoroutine(PlayerProgression.Instance.FadeAnim("fade in"));
            StartCoroutine(delayStart());
        }
    }

    IEnumerator delayStart()
    {
        yield return new WaitForSeconds(1f);
        DialogueSystem.Instance.StartDialogue(3);
        player.SetActive(false);
    }
}
