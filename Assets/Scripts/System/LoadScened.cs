using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScened : MonoBehaviour
{
    public int index;

    public void OnCLickSkip()
    {
        SceneManager.LoadScene(index);
    }
}
