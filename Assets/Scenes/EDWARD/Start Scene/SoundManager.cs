using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] public AudioSource[] BackgroundMusicSource, SFXSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(string name)
    {
        for (int i = 0; i < SFXSource.Length; i++)
        {
            if (SFXSource[i].name == name)
            {
                SFXSource[i].GetComponent<AudioSource>().Play();
            }
        }
    }

    public void StopSFX(string name)
    {
        for (int i = 0; i < SFXSource.Length; i++)
        {
            if (SFXSource[i].name == name)
            {
                SFXSource[i].GetComponent<AudioSource>().Stop();
            }
        }
    }

    public void PlayBGM(string name)
    {
        for (int i = 0; i < BackgroundMusicSource.Length; i++)
        {
            if (BackgroundMusicSource[i].name == name)
            {
                BackgroundMusicSource[i].GetComponent<AudioSource>().Play();
            }
        }
    }

    public void StopBGM(string name)
    {
        for (int i = 0; i < BackgroundMusicSource.Length; i++)
        {
            BackgroundMusicSource[i].GetComponent<AudioSource>().Stop();
        }
    }
}