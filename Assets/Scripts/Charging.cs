using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charging : MonoBehaviour
{
    private Hearts playerHearts; // Getting the players hearts
    private bool inRange;
    private Vector3 moveDirection;
    private float distance;
    private Vector3 targetPos;

    // Put [SerializeField] stuff here for cooldowns for charge, radius of looking, and damage dealt on hit
    [SerializeField] private bool isCharging = false;
    [SerializeField] private bool canCharge = true;
    [SerializeField] private float sightRange = 500f;
    [SerializeField] private float damageDealt;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float speed = 20f;
    [SerializeField] private GameObject player;
    void Start()
    {
        playerHearts = player.GetComponent<Hearts>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if player is within radius (u can copy code from shrines.cs where i use radius of 2 objects)
        distance = (transform.position - player.transform.position).sqrMagnitude;
        

        if (distance < sightRange) {
            
            if (!isCharging) {
                transform.LookAt(player.transform.position);
                targetPos = player.transform.position;
                moveDirection = (targetPos - transform.position).normalized;
            } 
            if (canCharge) {
                isCharging = true;
                transform.position += moveDirection * speed * Time.deltaTime;
            }
            if (Vector3.Distance(transform.position, targetPos) < 0.5f && isCharging) {
                StartCoroutine(Cooldown());
                
            }
        }


        // Transform.Rotate the enemy to look at player if within radius

        // Every cooldown (using the cooldown code in Shootingscript.cs) save the direction where the player is and stop updating that direction every frame, and then just charge directly at that last point
        
        // if it collides with the player, run the playerHearts.takeDamage(dmg); function

        // For making it not keep aligning, set a boolean for where u have it look at player for if it is charging, adn set it to true when charging, false when not

        // that should be it :)
    }

    IEnumerator Cooldown() {
        playerHearts.takeDamage(0.5f);
        isCharging = false;
        canCharge = false;
        yield return new WaitForSeconds(dashCooldown);   
        canCharge = true;
    }
}
