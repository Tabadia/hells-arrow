using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float speed = 1f;
    [SerializeField] Camera mainCam;
    [SerializeField] Camera secondaryCam;

    public ShootingScript shootingScript;

    void Start() {
        shootingScript = player.GetComponent<ShootingScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the raycast doesn't hit, just focus the camera on the player
        Vector3 zoomPoint = player.position;
        if (shootingScript.isCharging) {
            // Sending a ray from camera to mouse position
            RaycastHit hit;
            if (Physics.Raycast(secondaryCam.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                zoomPoint = hit.point;
            }
        }
        // Getting the average between the focus point and the player position so that the player doesn't go out of view
        Vector3 avgPosition = (player.position + zoomPoint) / 2;

        // Setting the camera's position
        mainCam.transform.position = Vector3.Lerp(transform.position, new Vector3(avgPosition.x + 10, transform.position.y, avgPosition.z + 10), Time.deltaTime * speed);
        
    }
}