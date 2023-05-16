using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SamuraiData
{
    public float positionX;
    public float positionY;
    public float positionZ;

    public SamuraiData(GameObject samurai)
    {
        positionX = samurai.transform.position.x;
        positionY = samurai.transform.position.y;
        positionZ = samurai.transform.position.z;
    }

    public override string ToString()
    {
        return positionX + ", " + positionY + ", " + positionZ;
    }
}