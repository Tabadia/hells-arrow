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
            if (pauseMenu.activeSelf) {
                Unpause();
            }
            else {
                Pause();
            }
        }    
    }

    public void Unpause() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public void Pause() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        AudioListener.pause = true;
    }
}
