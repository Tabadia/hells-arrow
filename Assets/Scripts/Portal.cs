using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nextScene;

    private GameObject player;
    private AsyncOperation sceneAsync;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene(){
        AsyncOperation scene = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        scene.allowSceneActivation = false;
        sceneAsync = scene;

        while (scene.progress < 0.9f)
        {
            Debug.Log("Loading scene " + " Progress: " + scene.progress);
            yield return null;
        }
        OnFinishedLoadingScene();
    }

    void enableScene(int index) {
        //Activate the Scene
        sceneAsync.allowSceneActivation = true;

        Scene sceneToLoad = SceneManager.GetSceneByName(nextScene);
        if (sceneToLoad.IsValid())
        {
            Debug.Log("Scene is Valid");
            SceneManager.MoveGameObjectToScene(player, sceneToLoad);
            SceneManager.SetActiveScene(sceneToLoad);
        }
    }

    void OnFinishedLoadingScene() {
        Debug.Log("Done Loading Scene");
        enableScene(2);
        Debug.Log("Scene Activated!");
    }
}
