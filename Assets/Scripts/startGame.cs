using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startGame : MonoBehaviour
{
    [SerializeField] private Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() =>
        {

            SceneManager.LoadScene("Ice Map");
        });
    }
}
