using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class GameData
{
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;
    public string[] enemies;
    public float[] enemyPositionsX;
    public float[] enemyPositionsY;
    public float[] enemyPositionsZ;
    public string[,] upgrades;
    public int score;

    public GameData(GameObject samurai, GameObject shrine, GameObject enemiesManager, GameObject scoreText)
    {
        playerPositionX = samurai.transform.position.x;
        playerPositionY = samurai.transform.position.y;
        playerPositionZ = samurai.transform.position.z;

        ShrineManager shrineScript = shrine.GetComponent<ShrineManager>();
        upgrades = shrineScript.upgrades;

        enemies = new string[enemiesManager.transform.childCount];
        enemyPositionsX = new float[enemiesManager.transform.childCount];
        enemyPositionsY = new float[enemiesManager.transform.childCount];
        enemyPositionsZ = new float[enemiesManager.transform.childCount];

        for (int i = 0; i < enemiesManager.transform.childCount; i++)
        {
            Transform child = enemiesManager.transform.GetChild(i);

            enemies[i] = child.name;
            enemyPositionsX[i] = child.position.x;
            enemyPositionsY[i] = child.position.y;
            enemyPositionsZ[i] = child.position.z;
            
        }

        score = int.Parse(scoreText.GetComponent<TextMeshProUGUI>().text);
    }

    public override string ToString()
    {
        string printprint = "";

        for (int i = 0; i < enemies.Length; i++)
        {
            Debug.Log(enemies[i] + ": " + enemyPositionsX[i] + ", " + enemyPositionsY[i] + ", " + enemyPositionsZ[i] + "\n");
        }

        for (int i = 0; i < upgrades.GetLength(0); i++)
        {
            Debug.Log(upgrades[i, 0] + ", " + upgrades[i, 1]);
        }

        return printprint;
    }
}