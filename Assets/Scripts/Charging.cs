using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charging : MonoBehaviour
{
    private Hearts playerHearts; // Getting the players hearts

    // Put [SerializeField] stuff here for cooldowns for charge, radius of looking, and damage dealt on hit
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // check if player is within radius (u can copy code from shrines.cs where i use radius of 2 objects)

        // Transform.Rotate the enemy to look at player if within radius

        // Every cooldown (using the cooldown code in Shootingscript.cs) save the direction where the player is and stop updating that direction every frame, and then just charge directly at that last point
        
        // if it collides with the player, run the playerHearts.takeDamage(dmg); function

        // For making it not keep aligning, set a boolean for where u have it look at player for if it is charging, adn set it to true when charging, false when not

        // that should be it :)
    }
}
