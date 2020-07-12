using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider soundSlider;

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("soundVolume", volume);
    }

    private void Update()
    {
        float musicVol, soundVol;
        audioMixer.GetFloat("musicVolume", out musicVol);
        audioMixer.GetFloat("soundVolume", out soundVol);

        musicSlider.value = musicVol;
        soundSlider.value = soundVol;
    }
}
