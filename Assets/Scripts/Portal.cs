using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private GameObject UI;

    private GameObject player;
    // private AsyncOperation sceneAsync;
    private GameObject spawnPoint;
    private Hearts playerHealth;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<Hearts>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            LoadScene();
        }
    }

    void LoadScene(){
        // AsyncOperation scene = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        // scene.allowSceneActivation = false;
        // sceneAsync = scene;

        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);

        // while (scene.progress < 0.9f)
        // {
            // Debug.Log("Loading scene " + " Progress: " + scene.progress);
            // yield return null;
        // }
        OnFinishedLoadingScene();
    }

    void OnFinishedLoadingScene() {
        //Activate the Scene
        Scene sceneToLoad = SceneManager.GetSceneByName(nextScene);
        if (sceneToLoad.IsValid())
        {
            var shrineManager = GameObject.FindGameObjectWithTag("ShrineManager").GetComponent<ShrineManager>();
            shrineManager.OnLoad(true);
            
            // spawnPoint = shrineManager.spawnPoint;
            // player.transform.position = spawnPoint.transform.position;
            player.transform.position = new Vector3(312.64f, 2.05f, 29.44f);
            playerHealth.ResetHearts();
        }
    }
}
