using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector cutsceneTimeline;
    public string gameplaySceneName;

    private bool isCutsceneFinished = false;

    void Start()
    {
        cutsceneTimeline.stopped += OnCutsceneFinished;
        cutsceneTimeline.Play();
    }

    void OnCutsceneFinished(PlayableDirector director)
    {
        isCutsceneFinished = true;
        LoadGameplayScene();
    }

    void Update()
    {
        if (isCutsceneFinished && Input.GetKeyDown(KeyCode.Space)) // Change this condition as needed
        {
            LoadGameplayScene();
        }
    }

    void LoadGameplayScene()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }
}
