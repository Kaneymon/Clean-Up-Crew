using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;

public class SoundSettingsManager : MonoBehaviour
{
    // this class provides functions for the sound settings menu to use.
    //i should rewrite this class so that its easy to rewire and add in new components... my brain is fried right now though, its simple enough, whatever.

    [SerializeField] UnityEvent onEnableEvent;
    [SerializeField] SoundSettingsProfile soundSettings;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] TMP_Dropdown microphoneOptions;
    public void PopulateMicrophoneDevicesDropdown(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();
        dropdown.options.Clear();

        if (Microphone.devices.Length == 0)
        {
            dropdown.interactable = false;
            dropdown.options.Add(new TMP_Dropdown.OptionData("No Microphones Found"));
            dropdown.value = 0;
            dropdown.RefreshShownValue();
            return;
        }

        dropdown.interactable = true;

        // Convert device names into OptionData list
        List<TMP_Dropdown.OptionData> dropOptions = new List<TMP_Dropdown.OptionData>();
        foreach (string device in Microphone.devices)
        {
            dropOptions.Add(new TMP_Dropdown.OptionData(device));
        }

        dropdown.AddOptions(dropOptions);
        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }

    public string GetCurrentSelection(TMP_Dropdown dropdown)
    {
        return dropdown.options[dropdown.value].text;
    }

    public void SaveSoundSettings()
    {
        float sfxVolumeCapped = Mathf.Clamp01(sfxVolumeSlider.value);
        float musicVolumeCapped = Mathf.Clamp01(musicVolumeSlider.value);
        float MasterVolumeCapped = Mathf.Clamp01(masterVolumeSlider.value);
        int micDeviceIndex = microphoneOptions.value;

        soundSettings.microphoneDeviceIndex = micDeviceIndex;
        soundSettings.masterVolume = MasterVolumeCapped;
        soundSettings.musicVolume = musicVolumeCapped;
        soundSettings.sfxVolume = sfxVolumeCapped;
        UpdateAudioSettingsComponents();
    }

    public void UpdateAudioSettingsComponents()
    {
        sfxVolumeSlider.value = soundSettings.sfxVolume;
        musicVolumeSlider.value = soundSettings.musicVolume;
        masterVolumeSlider.value = soundSettings.masterVolume;
    }

    private void OnEnable()
    {
        onEnableEvent.Invoke();
    }
}
