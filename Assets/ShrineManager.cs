using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random=UnityEngine.Random;

public class ShrineManager : MonoBehaviour {
    [SerializeField] private Button[] upOptions;
    [SerializeField] private TextMeshProUGUI[] optionText;
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private GameObject shrineText;
    [SerializeField] private GameObject upMenu;

    [SerializeField] public float upgradePoints = 0;

    private GameObject[] shrines;
    private string chosenUpgrade = "";
    private GameObject closest;
    
    public string[,] upgrades = {{"Exploding", "0"}, {"Multishot", "0"}, {"Piercing", "0"}, {"Flaming", "0"}, {"Arrow Speed", "0"}};
    public bool inMenu = false;

    ShootingScript shootingScript;
    Shrines shrineScript;

    void Start() {
        shrines = GameObject.FindGameObjectsWithTag("Shrine");
        shootingScript = GetComponent<ShootingScript>();
        shrineScript = GetComponent<Shrines>();

        upOptions[0].onClick.AddListener(() => {
            chosenUpgrade = optionText[0].text;
            UpgradeClicked(chosenUpgrade);
        });
        upOptions[1].onClick.AddListener(() => {
            chosenUpgrade = optionText[1].text;
            UpgradeClicked(chosenUpgrade);
        });
        upOptions[2].onClick.AddListener(() => {
            chosenUpgrade = optionText[2].text;
            UpgradeClicked(chosenUpgrade);
        });

    }

    void Update() {
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject s in shrines) {
            float curDistance = s.GetComponent<Shrines>().distance;
            if (curDistance < distance) {
                closest = s;
                distance = curDistance;
            }
        }
        if (distance < maxDistance) {
            shrineText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) {
                Time.timeScale = 0;
                AudioListener.pause = true;
                List<string> temp = new List<string>(0);
                for (int i = 0; i < upgrades.GetLength(0); i++){
                    temp.Add(upgrades[i,0] + " " + (int.Parse(upgrades[i,1]) + 1));
                }
                for (int i = 0; i < upOptions.Length; i++){
                    optionText[i].text = closest.GetComponent<Shrines>().upgrades[i,0] + " " + (int.Parse(closest.GetComponent<Shrines>().upgrades[i,1]) + 1);
                }

                inMenu = true;
                upMenu.SetActive(true);
            }
        }
        else {
            shrineText.SetActive(false);
        }
    }

    void UpgradeClicked(string chosenUpgrade) {
        if (upgradePoints >= 1f) {
            print(chosenUpgrade);
            for (int i = 0; i < upgrades.GetLength(0); i++) {
                if ((upgrades[i, 0] + " " + (int.Parse(upgrades[i, 1]) + 1)) == chosenUpgrade) {
                    upgrades[i, 1] = (int.Parse(upgrades[i, 1]) + 1).ToString(); 
                }
            }

            inMenu = false;
            Time.timeScale = 1;
            AudioListener.pause = false;
            upMenu.SetActive(false);

            upgradePoints -= 1f;
        }   
    }
}
