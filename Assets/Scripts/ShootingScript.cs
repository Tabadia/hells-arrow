using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    // way too many variables
    [SerializeField] private int cooldown = 1;
    [SerializeField] private float maxCharge = 1f;
    [SerializeField] private int arrowAmount = 1;
    [SerializeField] private float defaultBowStrength = 10f;
    [SerializeField] private float defaultArrowSpeed = 200f;
    [SerializeField] private GameObject prefab;
    private float timer;
    private bool cooldownActive;

    void Update()
    {
        // If it can shoot then check for how long it charged (time since charge - time at start of charge)
        if (!cooldownActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!cooldownActive)
                {
                    timer = Time.time;
                    // stop movement
                }
                else timer = 0;

            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!cooldownActive) Shoot(Time.time - timer);
                else timer = 0;
            }
        }
    }


    private void Shoot(float chargeTime)
    {
        // Sets values to minimum and max incase they get messed with, converts charge time to stats
        if (chargeTime > maxCharge) chargeTime = maxCharge;

        float bowStrength = defaultBowStrength;
        float arrowSpeed = defaultArrowSpeed;

        arrowSpeed *= chargeTime;
        bowStrength *= chargeTime;

        if (bowStrength < 1) { bowStrength = 1; }
        else if (bowStrength > 10) { bowStrength = 10; }
        if (arrowSpeed < 20) { arrowSpeed = 20; }
        else if (arrowSpeed > 200) { arrowSpeed = 200; }

        // Spawns arrow
        prefab.GetComponent<Arrow>().arrowSpeed = arrowSpeed;
        Instantiate(prefab, transform.position, transform.rotation);
        //to do: zoom camera towards cursor pos

        // Start cooldown
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        // Cooldown stuff
        cooldownActive = true;
        yield return new WaitForSeconds(cooldown);
        cooldownActive = false;
    }
}
