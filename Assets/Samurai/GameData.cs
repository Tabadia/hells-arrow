using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameData(GameObject samurai, GameObject shrine, GameObject enemiesManager)
    {
        playerPositionX = samurai.transform.position.x;
        playerPositionY = samurai.transform.position.y;
        playerPositionZ = samurai.transform.position.z;
        Debug.Log(enemiesManager.name);

        enemies = new string[enemiesManager.transform.childCount];
        enemyPositionsX = new float[enemiesManager.transform.childCount];
        enemyPositionsY = new float[enemiesManager.transform.childCount];
        enemyPositionsZ = new float[enemiesManager.transform.childCount];

        for (int i = 0; i < enemiesManager.transform.childCount; i++)
        {
            Transform child = enemiesManager.transform.GetChild(i);
            Debug.Log(child.name);
            Debug.Log(child.position.x);
            Debug.Log(child.position.y);
            Debug.Log(child.position.z);

            enemies[i] = child.name;
            enemyPositionsX[i] = child.position.x;
            enemyPositionsY[i] = child.position.y;
            enemyPositionsZ[i] = child.position.z;
            
        }
    }

    public override string ToString()
    {
        string printprint = "";

        for (int i = 0; i < enemies.Length; i++)
        {
            printprint += enemies[i] + ": " + enemyPositionsX[i] + ", " + enemyPositionsY[i] + ", " + enemyPositionsZ[i] + "\n";
        }

        return printprint;
    }
}