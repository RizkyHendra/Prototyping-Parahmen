using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider audioSlider, musicSlider;
    public AudioSource audioSource, musicSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Audio", 1);
        musicSource.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music", 0.5f);
        audioSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Audio",1);
        musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Music", 0.5f);
    }

    // Update is called once per frame
    public void AudioControl()
    {
        audioSource.GetComponent<AudioSource>().volume = audioSlider.GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("Audio", audioSlider.GetComponent<Slider>().value);
    }
    public void MusicControl()
    {
        musicSource.GetComponent<AudioSource>().volume = musicSlider.GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("Music", musicSlider.GetComponent<Slider>().value);
    }
}
