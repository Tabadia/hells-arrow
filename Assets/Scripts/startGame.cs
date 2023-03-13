using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour
{
    [SerializeField] private Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() => {
            SceneManager.LoadScene("Test Scene");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
