using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public bool isFullscreen = true;
    public GameObject fullscreenOn;
    public GameObject fullscreenOff;
    public AudioMixer audioMixer;
    private Resolution[] resolutions;
    private int currentResolutionIndex = 0;

    void Start()
    {
        Screen.fullScreen = isFullscreen;
        resolutions = Screen.resolutions;
        dropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionOption = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(resolutionOption);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = currentResolutionIndex;
        dropdown.RefreshShownValue();
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Lerp(-80, 20, volume));
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Level", Mathf.Lerp(-80, 20, volume));
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
    }

    public void FullScreen()
    {
        fullscreenOff.SetActive(!fullscreenOff.activeSelf);
        fullscreenOn.SetActive(!fullscreenOn.activeSelf);
        isFullscreen = !isFullscreen;
        Screen.fullScreen = fullscreenOn.activeSelf;
    }
}
