using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource[] audioSourcesToIgnore;
    public float playerScore = 0f;

    void Start() {
        foreach(AudioSource audioSource in audioSourcesToIgnore) {
            audioSource.ignoreListenerPause = true;
        }
    }

    void Update() {
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

    public void Pause()
    {
        pauseMenu.SetActive(true);
        scoreText.GetComponent<TextMeshProUGUI>().text = playerScore.ToString("g2");
        Time.timeScale = 0;
        AudioListener.pause = true;
    }
}
