using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [FormerlySerializedAs("scoreText")] [SerializeField] private TextMeshProUGUI scoreVar;
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
        SaveLoad.SaveData(samurai, shrineManager, enemies, scoreVar.gameObject);
        Unpause();
    }

    public void Exit()
    {
        Unpause();
        SceneManager.LoadScene("Start");
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
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
