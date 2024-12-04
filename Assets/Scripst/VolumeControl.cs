using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundEffectsVolumeSlider;

    void Start()
    {
        
        // Add listeners to the sliders
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        soundEffectsVolumeSlider.onValueChanged.AddListener(SetSoundEffectsVolume);

        musicVolumeSlider.value = AudioManager.Instance.backgroundMusicVolume;
        soundEffectsVolumeSlider.value = AudioManager.Instance.soundEffectsVolume;
    }

    void SetMusicVolume(float volume)
    {
        AudioManager.Instance?.SetBackgroundMusicVolume(volume);
    }

    void SetSoundEffectsVolume(float volume)
    {
        AudioManager.Instance?.SetSoundEffectsVolume(volume);
    }
}