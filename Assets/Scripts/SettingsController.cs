using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Button exitButton;
    [SerializeField] private AudioMixer audioMixer;

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void Open()
    {
        settingsMenu.SetActive(true);
    }

    public void Exit()
    {
        settingsMenu.SetActive(false);
    }
}
