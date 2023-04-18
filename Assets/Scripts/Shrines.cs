using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random=UnityEngine.Random;

public class Shrines : MonoBehaviour {

    [SerializeField] private  int upgradeAmt = 3;

    public string[,] upgradeOptions;
    public ShrineManager shrineManager;
    public string[,] upgrades = new string[3, 2];
    public float distance = Mathf.Infinity;

    private GameObject player;

    void Start() {
        shrineManager = GameObject.FindGameObjectWithTag("ShrineManager").GetComponent<ShrineManager>();
        upgradeOptions = shrineManager.upgrades;
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < upgradeAmt; i++){
            upgrades[i,0] = upgradeOptions[Random.Range(0, upgradeOptions.GetLength(0)), 0];
            bool unique = true;
            while (true){
                for (int j = 0; j < i; j++){
                    if (upgrades[i,0] == upgrades[j,0]){
                        unique = false;
                    }
                }
                if (unique){
                    break;
                }
                upgrades[i,0] = upgradeOptions[Random.Range(0, upgradeOptions.GetLength(0)), 0];
            }
            upgrades[i,1] = "0";
        }
    }

    void Update() {
        distance = Vector3.Distance(player.transform.position, transform.position);
    }
}
