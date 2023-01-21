using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    private Vector3 newtrans;

    void Start ()
    {
        offset.x = transform.position.x - player.transform.position.x;
        offset.z = transform.position.z - player.transform.position.z;
        newtrans=transform.position;

    }
    void LateUpdate ()
    {
        newtrans.x= player.transform.position.x + offset.x;
        newtrans.z= player.transform.position.z + offset.z;
        transform.position = newtrans;
    }

}