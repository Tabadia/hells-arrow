using TMPro;
using UnityEngine;

public class StartUI : MonoBehaviour {

    [SerializeField] private TMP_Text creditText;
    [SerializeField] private GameObject creditScreen;

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
