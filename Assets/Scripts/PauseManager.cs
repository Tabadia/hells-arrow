using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioMixer audioMixer;

    private AudioSource[] allAudioSources;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
                foreach(AudioSource audioS in allAudioSources){
                    audioS.Stop();
                }
            }
        }    
    }

    public void Unpause() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
