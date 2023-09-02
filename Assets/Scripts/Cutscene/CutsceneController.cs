using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector cutsceneTimeline;
    public string gameplaySceneName;
    public GameObject fadeEffect;

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

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(delayFade());
        }
    }

    void LoadGameplayScene()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    IEnumerator delayFade()
    {
        fadeEffect.SetActive(true);
        fadeEffect.GetComponent<Animator>().Play("fade in");
        yield return new WaitForSeconds(1f);
        LoadGameplayScene();
    }
}
