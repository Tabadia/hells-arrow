using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random=UnityEngine.Random;

public class Shrines : MonoBehaviour {
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private GameObject shrineText;
    [SerializeField] private GameObject upMenu;
    [SerializeField] private Button upOption1;
    [SerializeField] private Button upOption2;
    [SerializeField] private Button upOption3;
    [SerializeField] public float upgradePoints = 0;

    private GameObject[] shrines;
    private string chosenUpgrade = "";
    private TextMeshProUGUI optionText1;
    private TextMeshProUGUI optionText2;
    private TextMeshProUGUI optionText3;
    
    public string[,] upgrades = {{"Exploding", "0"}, {"Multishot", "0"}, {"Piercing", "0"}, {"Flaming", "0"}, {"Arrow Speed", "0"}};
    ShootingScript shootingScript;
    public bool inMenu = false;

    void Start() {
        shrines = GameObject.FindGameObjectsWithTag("Shrine");
        shootingScript = GetComponent<ShootingScript>();
        optionText1 = upOption1.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        optionText2 = upOption2.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        optionText3 = upOption3.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        upOption1.onClick.AddListener(() => {
            chosenUpgrade = optionText1.text;
            UpgradeClicked(chosenUpgrade);
        });
        upOption2.onClick.AddListener(() => {
            chosenUpgrade = optionText2.text;
            UpgradeClicked(chosenUpgrade);
        });
        upOption3.onClick.AddListener(() => {
            chosenUpgrade = optionText3.text;
            UpgradeClicked(chosenUpgrade);
        });
    }

    void Update() {
        GameObject closest = null;
        float distance = Mathf.Infinity;

        Vector3 position = transform.position;
        foreach (GameObject s in shrines) {
            Vector3 diff = s.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                closest = s;
                distance = curDistance;
            }
        }

        // If close to shrine
        if (distance < maxDistance) {
            //print("near shrine");
            shrineText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) {
                List<string> temp = new List<string>(0);
                for (int i = 0; i < upgrades.GetLength(0); i++){
                    print((int.Parse(upgrades[i,1]) + 1).ToString());
                    temp.Add(upgrades[i,0] + " " + (int.Parse(upgrades[i,1]) + 1));
                }
                int rand = 0;
                
                rand = Random.Range(0, temp.Count);
                optionText1.text = temp[rand];
                temp.RemoveAt(rand);

                rand = Random.Range(0, temp.Count);
                optionText2.text = temp[rand];
                temp.RemoveAt(rand);

                rand = Random.Range(0, temp.Count);
                optionText3.text = temp[rand];
                temp.RemoveAt(rand);

                print("in menu");
                inMenu = true;
                upMenu.SetActive(true);
            }

            //print("Closest shrine is " + closest.name + " at " + distance + " units away");
        }
        else {
            shrineText.SetActive(false);
        }
    }

    void UpgradeClicked(string chosenUpgrade)
    {
        if (upgradePoints >= 1f) {
            print(chosenUpgrade);
            for (int i = 0; i < upgrades.GetLength(0); i++)
            {
                if ((upgrades[i, 0] + " " + (int.Parse(upgrades[i, 1]) + 1)) == chosenUpgrade)
                {
                    upgrades[i, 1] = (int.Parse(upgrades[i, 1]) + 1).ToString();
                }
            }

            inMenu = false;
            upMenu.SetActive(false);

            upgradePoints -= 1f;
        }   
    }
}
