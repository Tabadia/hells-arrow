using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartUI : MonoBehaviour {

    [SerializeField] private TMP_Text creditText;
    [SerializeField] private GameObject creditScreen;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ToggleCredits(){
        if (creditText.text == "Credits")
        {
            creditText.text = "Close";
            creditScreen.SetActive(true);
        }
        else
        {
            creditText.text = "Credits";
            creditScreen.SetActive(false);
        }
    }
}
