using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    [SerializeField] private GameObject samurai;
    [SerializeField] private GameObject shrineManager;
    [SerializeField] private GameObject enemies;
    [SerializeField] private bool shouldLoad;
    [SerializeField] private GameObject scoreVar;
    [SerializeField] private PauseManager pauseManager;

    // Start is called before the first frame update
    void Start()
    {
        if (!shouldLoad) return;

        GameData loadData = SaveLoad.LoadData();
        samurai.transform.position = new Vector3(loadData.playerPositionX,
                                                 loadData.playerPositionY,
                                                 loadData.playerPositionZ);

        shrineManager.transform.GetComponent<ShrineManager>().upgrades = loadData.upgrades;
        pauseManager.playerScore = loadData.score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
