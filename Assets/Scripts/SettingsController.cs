using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    // [SerializeField] private Button exitButton;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start() {
        // Add good resolution options to the dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        
        List<string> resOptions = new List<string>();
        
        int currentRes = 0;

        for (int i = 0; i < resolutions.Length; i++) {
            string res = resolutions[i].width + " x " + resolutions[i].height;
            if (!resOptions.Contains(res)){
                resOptions.Add(res);
            }

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                currentRes = i;
            }
        }

        resolutionDropdown.AddOptions(resOptions);
        resolutionDropdown.value = currentRes;
        resolutionDropdown.RefreshShownValue();
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, Screen.fullScreen);
    }
    
    // public functions that are called by buttons in the settings menu

    public void SetVolume (float volume) {
        audioMixer.SetFloat("volume", volume);
    }

    public void Open() {
        settingsMenu.SetActive(true);
    }

    public void Exit() {
        settingsMenu.SetActive(false);
    }

    public void ToggleFullscreen(){
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, !Screen.fullScreen);
    }

    public void SetResolution(int resIndex) {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityInt){
        QualitySettings.SetQualityLevel(qualityInt);
    }

}
