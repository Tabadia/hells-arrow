using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nextScene;

    private Hearts playerHealth;

    // void Start(){
    //     player = GameObject.FindGameObjectWithTag("Player");
    //     playerHealth = player.GetComponent<Hearts>();
    // }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
