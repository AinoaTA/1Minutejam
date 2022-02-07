using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioMixer TheMixer;
    public Slider musicSlider, sfxSlider;
    public TMP_Text musicLabel, sfxLabel;

    private void Start()
    {
        SetMusicVolume();
        SetSFXVolume();
        SetMasterVolume();
    }
    public void SetMasterVolume()
    {
        TheMixer.SetFloat("MasterValue", 0);
    }
    public void SetMusicVolume()
    {
        musicLabel.text = (musicSlider.value + 80).ToString();
        TheMixer.SetFloat("MusicValue", musicSlider.value);
    }

    public void SetSFXVolume()
    {
        sfxLabel.text = (sfxSlider.value + 80).ToString();
        TheMixer.SetFloat("SFXValue", (sfxSlider.value));
    }


}
