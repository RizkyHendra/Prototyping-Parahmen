using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedYPosition : MonoBehaviour
{
    public float YPosition;
    void Update()
    {
        transform.position = new Vector3(transform.position.x, YPosition, transform.position.z);
    }
}
