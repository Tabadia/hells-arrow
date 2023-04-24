using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random=UnityEngine.Random;

public class Shrines : MonoBehaviour {

    [SerializeField] private  int upgradeAmt = 3;

    public string[,] upgradeOptions;
    private ShrineManager shrineManager;
    public string[,] upgrades = new string[3, 2];
    public float distance = 200;

    private GameObject player;

    void Start() {
        shrineManager = GameObject.FindGameObjectWithTag("ShrineManager").GetComponent<ShrineManager>();
        upgradeOptions = shrineManager.upgrades;
        player = GameObject.FindGameObjectWithTag("Player");
        
        string[,] tempArr = new string[upgradeOptions.GetLength(0), 2];
        for (int i = 0; i < upgradeOptions.GetLength(0); i++){
            tempArr[i,0] = upgradeOptions[i,0];
            tempArr[i,1] = upgradeOptions[i,1];
        }
        for (int i = 0; i < upgradeAmt; i++){
            int r = Random.Range(0, tempArr.GetLength(0));
            if (tempArr[r,0] != "a"){
                upgrades[i,0] = tempArr[r,0];
                upgrades[i,1] = "0";
                tempArr[r,0] = "a";
            }
            else{
                i--;
            }
        }
    }

    void Update() {
        distance = Vector3.Distance(player.transform.position, transform.position);
    }
}
