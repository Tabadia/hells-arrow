using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    // way too many variables
    [SerializeField] private float cooldown = .5f;
    [SerializeField] private float maxCharge = 1f;
    [SerializeField] public int arrowAmount = 1;
    [SerializeField] public int pierceAmount = 0;
    [SerializeField] public bool exploding = false;
    [SerializeField] private float minBowStrength = 1f;
    [SerializeField] private float maxBowStrength = 10f;
    [SerializeField] private float minArrowSpeed = 50f;
    [SerializeField] private float maxArrowSpeed = 400f;
    [SerializeField] private float multishotAngle = 5;
    [SerializeField] private GameObject prefab;
    [SerializeField] private ParticleSystem maxChargeParticleSystem;
    [SerializeField] private Light maxChargeLight;

    private float timer;
    private float fullParticleTimer;
    private bool spawnedMaxParticle;
    private bool cooldownActive;

    public bool isCharging = false;

    void Update()
    {
        var emission = maxChargeParticleSystem.emission;

        // If it can shoot then check for how long it charged (time since charge - time at start of charge)
        if (!cooldownActive)
        {
            if (Input.GetMouseButtonDown(0)) {
                if (!cooldownActive) {
                    timer = Time.time;
                    isCharging = true;
                }
                else timer = 0;

            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!cooldownActive && isCharging) {
                    Shoot(Time.time - timer);
                }
                else timer = 0;
                isCharging = false;

                spawnedMaxParticle = false;
            }
        }

        // Increment by time.deltatime when charging
        if (isCharging)
            fullParticleTimer += Time.deltaTime;
        else 
            fullParticleTimer = 0;

        // Spawn a particle if the charge time is more than maxCharge
        
        emission.rateOverTime = Mathf.Lerp(0, 20, fullParticleTimer);
        maxChargeLight.intensity = Mathf.Lerp(0, 10, fullParticleTimer);
    }


    private void Shoot(float chargeTime)
    {
        // Sets values to minimum and max incase they get messed with, converts charge time to stats
        if (chargeTime > maxCharge) chargeTime = maxCharge;

        float bowStrength = maxBowStrength;
        float arrowSpeed = maxArrowSpeed;

        arrowSpeed *= chargeTime;
        bowStrength *= chargeTime;

        if (arrowSpeed < minArrowSpeed) arrowSpeed = minArrowSpeed;
        else if (arrowSpeed > maxArrowSpeed) arrowSpeed = maxArrowSpeed;
        if (bowStrength < minBowStrength) bowStrength = minBowStrength;
        else if (bowStrength > maxBowStrength) bowStrength = maxBowStrength;

        // Spawns arrow
        if (arrowAmount > 1)
        {
            bowStrength /= 1.5f;

            if (arrowSpeed < minArrowSpeed) arrowSpeed = minArrowSpeed;
            else if (arrowSpeed > maxArrowSpeed) arrowSpeed = maxArrowSpeed;
            if (bowStrength < minBowStrength) bowStrength = minBowStrength / 2;
            else if (bowStrength > maxBowStrength) bowStrength = maxBowStrength / 2;

            float spacing = -(multishotAngle * arrowAmount / 4);
            for (int i = 0; i < arrowAmount; i++)
            {
                prefab.GetComponent<Arrow>().multishotAngle = spacing;
                prefab.GetComponent<Arrow>().pierceAmount = pierceAmount;
                prefab.GetComponent<Arrow>().arrowSpeed = arrowSpeed;
                prefab.GetComponent<Arrow>().bowStrength = bowStrength;
                prefab.GetComponent<Arrow>().exploding = exploding;
                Instantiate(prefab, transform.position, transform.rotation);
                spacing += multishotAngle;
            }
        }
        else {
            prefab.GetComponent<Arrow>().multishotAngle = 0;
            prefab.GetComponent<Arrow>().pierceAmount = pierceAmount;
            prefab.GetComponent<Arrow>().arrowSpeed = arrowSpeed;
            prefab.GetComponent<Arrow>().bowStrength = bowStrength;
            prefab.GetComponent<Arrow>().exploding = exploding;
            Instantiate(prefab, transform.position, transform.rotation);
        }

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
