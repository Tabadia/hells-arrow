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
    [SerializeField] private float minBowStrength = 1f;
    [SerializeField] private float maxBowStrength = 10f;
    [SerializeField] private float minArrowSpeed = 50f;
    [SerializeField] private float maxArrowSpeed = 400f;
    [SerializeField] private GameObject prefab;

    private float timer;
    private bool cooldownActive;
    private bool multishot = false;

    public bool isCharging = false;

    void Update()
    {
        // If it can shoot then check for how long it charged (time since charge - time at start of charge)
        if (GetComponent<Shrines>().powerUps.Contains("Multishot")) multishot = true;

        if (!cooldownActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!cooldownActive)
                {
                    timer = Time.time;
                    isCharging = true;
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

        if (arrowSpeed < minArrowSpeed) arrowSpeed = minArrowSpeed;
        else if (arrowSpeed > maxArrowSpeed) arrowSpeed = maxArrowSpeed;

        // Spawns arrow
        if (multishot)
        {
            bowStrength *= chargeTime / 2;
            if (bowStrength < minBowStrength) bowStrength = minBowStrength / 2;
            else if (bowStrength > maxBowStrength) bowStrength = maxBowStrength / 2;

            prefab.GetComponent<Arrow>().arrowSpeed = arrowSpeed;
            prefab.GetComponent<Arrow>().multiShotArrow = 1;
            Instantiate(prefab, transform.position, transform.rotation);

            prefab.GetComponent<Arrow>().arrowSpeed = arrowSpeed;
            prefab.GetComponent<Arrow>().multiShotArrow = 2;
            Instantiate(prefab, transform.position, transform.rotation);

            prefab.GetComponent<Arrow>().arrowSpeed = arrowSpeed;
            prefab.GetComponent<Arrow>().multiShotArrow = 3;
            Instantiate(prefab, transform.position, transform.rotation);
        }
        else
        {
            prefab.GetComponent<Arrow>().multiShotArrow = 1;
            prefab.GetComponent<Arrow>().arrowSpeed = arrowSpeed;
            Instantiate(prefab, transform.position, transform.rotation);
        }

        //to do: zoom camera towards cursor pos (maybe)

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
