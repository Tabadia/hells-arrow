using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random=UnityEngine.Random;

public class Shrines : MonoBehaviour {

    [SerializeField] private  int upgradeAmt = 3;

    public string[,] upgradeOptions = {{"Exploding", "0"}, {"Multishot", "0"}, {"Piercing", "0"}, {"Flaming", "0"}, {"Arrow Speed", "0"}};
    
    public string[,] upgrades = new string[3, 2];
    public float distance = Mathf.Infinity;

    private GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < upgradeAmt; i++){
            upgrades[i,0] = upgradeOptions[Random.Range(0, upgradeOptions.GetLength(0)), 0];
            upgrades[i,1] = "0";
        }
    }

    void Update() {
        distance = Vector3.Distance(player.transform.position, transform.position);
    }
}
