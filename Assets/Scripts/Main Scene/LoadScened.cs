using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScened : MonoBehaviour
{
    public int index;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.SetInt("Progress", PlayerPrefs.GetInt("Progress") + 1);
            SceneManager.LoadScene(index);
        }
    }
}
