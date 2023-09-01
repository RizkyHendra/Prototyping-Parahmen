using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public bool isPaused;

    private void Start()
    {
        PauseToggle(false);
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                PauseToggle(false);
            }
            else
            {
                PauseToggle(true);
            }
        }
    }

    private void PauseToggle(bool state)
    {
        if (state == true)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}
