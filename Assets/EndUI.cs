using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndUI : MonoBehaviour
{
    [SerializeField] private GameObject creditScreen;
    private bool isOpen;

    public void ToggleCredits(){
        if (creditScreen.activeSelf){
            creditScreen.SetActive(false);
        }
        else{
            creditScreen.SetActive(true);
        }
    }

    public void mainMenu(){
        SceneManager.LoadScene("Start");
    }
}
