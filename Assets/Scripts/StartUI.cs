using TMPro;
using Unity.VisualScripting;
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

    void Awake()
    {
        if (!GameObject.FindWithTag("Player").IsUnityNull())
        {
            var player = GameObject.FindWithTag("Player");
            var ui = GameObject.FindWithTag("UIManager");
            var cameraController = player.GetComponent<MovementController>().cameraMain.gameObject
                .GetComponent<CameraController>();
            Destroy(cameraController.mainCam.gameObject);
            Destroy(cameraController.secondaryCam.gameObject);
            Destroy(player);
            Destroy(ui);
        }
    }
}
