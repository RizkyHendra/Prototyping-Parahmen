using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject[] inGame;
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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void PauseToggle(bool state)
    {
        if (state == true)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;

            DialogueSystem.Instance.player.GetComponent<PlayerInput>().DeactivateInput();

            for (int i = 0; i < inGame.Length; i++)
            {
                inGame[i].SetActive(false);
            }
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;

            DialogueSystem.Instance.player.GetComponent<PlayerInput>().ActivateInput();

            for (int i = 0; i < inGame.Length; i++)
            {
                inGame[i].SetActive(true);
            }
        }
    }

    public void LoadToScene(int index)
    {
        SceneManager.LoadScene(index);
        Time.timeScale = 1;
    }

    public void ResumeButton()
    {
        PauseToggle(false);
    }
}
