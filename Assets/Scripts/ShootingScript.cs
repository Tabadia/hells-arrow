using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    // way too many variables
    [SerializeField] private float cooldown = .5f;
    [SerializeField] private float maxCharge = 1f;
    //[SerializeField] private int arrowAmount = 1;
    [SerializeField] private float maxBowStrength = 10f;
    [SerializeField] private float maxArrowSpeed = 400f;
    [SerializeField] private GameObject prefab;

    private float timer;
    private bool cooldownActive;

    public bool isCharging = false;

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
                    isCharging = true;
                    print("charging...");
                }
                else timer = 0;

            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!cooldownActive && isCharging)
                {
                    Shoot(Time.time - timer);
                }
                else timer = 0;
                isCharging = false;
            }
        }
    }


    private void Shoot(float chargeTime)
    {
        // Sets values to minimum and max incase they get messed with, converts charge time to stats
        if (chargeTime > maxCharge) chargeTime = maxCharge;

        float bowStrength = maxBowStrength;
        float arrowSpeed = maxArrowSpeed;

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
