using TMPro;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject scoreText;
    // [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource[] audioSourcesToIgnore;
    [SerializeField] private GameObject samurai;
    [SerializeField] private GameObject shrineManager;
    [SerializeField] private GameObject enemies;
    [SerializeField] private AudioSource UImusic;

    public float playerScore;

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

    public void Save()
    {
        SaveLoad.SaveData(samurai, shrineManager, enemies, scoreText);
        Unpause();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        scoreText.GetComponent<TextMeshProUGUI>().text = playerScore.ToString("g2");
        Time.timeScale = 0;
        AudioListener.pause = true;
        UImusic.Play(0);
    }

    public void Unpause() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false;
        UImusic.Stop();
    }

}
