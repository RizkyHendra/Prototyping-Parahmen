using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatesController : MonoBehaviour
{
    public GameObject Gates;
    private Animator gatesAnim;
    void Start()
    {
        gatesAnim = Gates.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            gatesAnim.Play("open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(delayClose());
        }
    }

    private IEnumerator delayClose()
    {
        gatesAnim.Play("close");
        yield return new WaitForSeconds(2f);
        gatesAnim.Play("gates");
    }
}
