using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalLorongStopCharacter : MonoBehaviour
{
    public bool stop;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stop = true;
            Debug.Log("Stopppp");
        }
    }
}
