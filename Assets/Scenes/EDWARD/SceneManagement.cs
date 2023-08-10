using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void changeScene(int idScene)
    {
        SceneManager.LoadScene(idScene);
    }
    public void exitGame()
    {
        Application.Quit();
    }


}
