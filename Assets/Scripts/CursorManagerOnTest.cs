using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManagerOnTest : MonoBehaviour
{
    void Start()
    {
        EnableCursorMouse();
    }

    public void EnableCursorMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
