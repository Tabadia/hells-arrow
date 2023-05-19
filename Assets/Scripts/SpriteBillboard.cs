using UnityEngine;

//This script makes any gameobject look at the camera
public class SpriteBillboard : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}
