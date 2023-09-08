using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Start()
    {
        for (int i = 0; i < SoundManager.Instance.BackgroundMusicSource.Length; i++)
        {
            SoundManager.Instance.BackgroundMusicSource[i].volume = PlayerPrefs.GetFloat("BGMSetting", 1f);
            bgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMSetting", 1f);
        }
        for (int i = 0; i < SoundManager.Instance.SFXSource.Length; i++)
        {
            SoundManager.Instance.SFXSource[i].volume = PlayerPrefs.GetFloat("SFXSetting", 1f);
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXSetting", 1f);
        }
    }

    public void SetBGMVolume()
    {
        for (int i = 0; i < SoundManager.Instance.BackgroundMusicSource.Length; i++)
        {
            SoundManager.Instance.BackgroundMusicSource[i].volume = bgmVolumeSlider.value;
            PlayerPrefs.SetFloat("BGMSetting", bgmVolumeSlider.value);
        }
    }

    public void SetSFXVolume()
    {
        for (int i = 0; i < SoundManager.Instance.SFXSource.Length; i++)
        {
            SoundManager.Instance.SFXSource[i].volume = sfxVolumeSlider.value;
            PlayerPrefs.SetFloat("SFXSetting", sfxVolumeSlider.value);
        }
    }
}