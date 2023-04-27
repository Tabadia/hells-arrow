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

    private float posDif;
    private Quaternion rotation;
    private bool movingCamera;

    void Start() {
        shootingScript = player.GetComponent<ShootingScript>();
        rotation = transform.rotation;
        posDif = 10f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Keep camera rotation static
        mainCam.transform.rotation = rotation; 
        
        // If the raycast doesn't hit, just focus the camera on the player
        Vector3 zoomPoint = player.position;
        if (shootingScript.isCharging) {
            // Sending a ray from camera to mouse position
            RaycastHit hit;
            if (Physics.Raycast(secondaryCam.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                zoomPoint = hit.point;
                Debug.Log(hit.collider.gameObject.name);
            }
        }
        // Getting the average between the focus point and the player position so that the player doesn't go out of view
        Vector3 avgPosition = (player.position + zoomPoint) / 2;

        // Check if camera is already in default position
        bool homedCam =
            Vector3.Distance(new Vector3(mainCam.transform.position.x - 10, player.transform.position.y, mainCam.transform.position.z - 10),
            player.transform.position) < 0.1f;
        //Debug.Log(homedCam);

        // Setting the camera's position
        // Move to the focus point when beginning charge - this if-else tree could probably be structured better, but fuk u
        if (zoomPoint != player.transform.position)
        {
            movingCamera = true;
            mainCam.transform.position = Vector3.Lerp(transform.position,
                new Vector3(avgPosition.x + posDif, transform.position.y, avgPosition.z + posDif),
                Time.deltaTime * speed);
        }
        // Move back to the player when releasing
        else if (zoomPoint == player.transform.position && movingCamera && !homedCam)
        {
            var speedModifier = Mathf.Max(Vector3.Distance(new Vector3(mainCam.transform.position.x - 10, player.transform.position.y, mainCam.transform.position.z - 10)
                , player.transform.position)*2f, 1.5f); // Basically, in order to catch the moving player when returning, camera moves faster the further it is
            
            mainCam.transform.position = Vector3.Lerp(transform.position,
                new Vector3(avgPosition.x + posDif, transform.position.y, avgPosition.z + posDif),
                Time.deltaTime * speed * speedModifier); 
        }
        else switch (homedCam) // Different behavior when the camera is homed on player
        {
            case true when movingCamera: // Runs when camera returns after charging
                movingCamera = false;
                // Debug.Log("checkMC");
                break;
            case false when !movingCamera: // Check here is needed to make sure camera can leave the player the first time
                mainCam.transform.position = new Vector3(player.transform.position.x + 10, transform.position.y,
                    player.transform.position.z + 10);
                //Debug.Log("check");
                break;
        }
        
        secondaryCam.transform.position = new Vector3(player.transform.position.x + 10, transform.position.y,
            player.transform.position.z + 10);
    }
}