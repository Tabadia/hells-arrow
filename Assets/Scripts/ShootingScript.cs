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
    [SerializeField] public bool flame = false;
    [SerializeField] private int flameLength = 0;
    [SerializeField] private float minBowStrength = 1f;
    [SerializeField] private float maxBowStrength = 10f;
    [SerializeField] private float minArrowSpeed = 50f;
    [SerializeField] private float maxArrowSpeed = 400f;
    [SerializeField] private float multishotAngle = 5;
    [SerializeField] private GameObject prefab;
    [SerializeField] private ParticleSystem maxChargeParticleSystem;
    [SerializeField] private Light maxChargeLight;
    [SerializeField] private AudioSource chargeSFX;
    [SerializeField] private AudioSource shootSFX;
    [SerializeField] private GameObject shrinesObject;
    [SerializeField] private Animator samuraiAnimator;

    private float timer;
    private float fullParticleTimer;
    private bool spawnedMaxParticle;
    private bool cooldownActive;
    private ShrineManager shrineScript;
    public string[,] upgrades;

    public bool isCharging = false;

    void Start() {
        shrineScript = shrinesObject.GetComponent<ShrineManager>();
        upgrades = shrineScript.upgrades;
    }

    void Update() {
        var emission = maxChargeParticleSystem.emission;

        // If it can shoot then check for how long it charged (time since charge - time at start of charge)
        if (!cooldownActive)
        {
            if (Input.GetMouseButtonDown(0)) {
                if (!cooldownActive) {
                    timer = Time.time;
                    isCharging = true;
                    chargeSFX.Play();
                }
                else timer = 0;

            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!cooldownActive && isCharging) {
                    Shoot(Time.time - timer);
                    chargeSFX.Stop();
                    shootSFX.Play();
                }
                else timer = 0;
                isCharging = false;

                spawnedMaxParticle = false;
            }
        }

        // Increment by time.deltatime when charging
        if (isCharging)
        {
            fullParticleTimer += Time.deltaTime;
            samuraiAnimator.SetBool("IsCharging", true);
        }
        else
        {
            fullParticleTimer = 0;
            samuraiAnimator.SetBool("IsCharging", false);
        }

        // Spawn a particle if the charge time is more than maxCharge

        emission.rateOverTime = Mathf.Lerp(0, 20, fullParticleTimer);
        maxChargeLight.intensity = Mathf.Lerp(0, 10, fullParticleTimer);
    }


    private void Shoot(float chargeTime) {

        // Sets values to minimum and max incase they get messed with, converts charge time to stats
        if (chargeTime > maxCharge) chargeTime = maxCharge;

        float bowStrength = maxBowStrength;
        float arrowSpeed = maxArrowSpeed;
        float speedMult = 1;

        arrowSpeed *= chargeTime;
        bowStrength *= chargeTime;

        if (arrowSpeed < minArrowSpeed) arrowSpeed = minArrowSpeed;
        else if (arrowSpeed > maxArrowSpeed) arrowSpeed = maxArrowSpeed;
        if (bowStrength < minBowStrength) bowStrength = minBowStrength;
        else if (bowStrength > maxBowStrength) bowStrength = maxBowStrength;

        // Get all upgrades
        upgrades = shrineScript.upgrades;

        for (int i = 0; i < upgrades.GetLength(0); i++){
            if(upgrades[i, 0] == "Exploding" && int.Parse(upgrades[i, 1]) > 0) {
                exploding = true;
                bowStrength *= (1.1f * float.Parse(upgrades[i, 1]));
            }
            else if(upgrades[i, 0] == "Multishot" && int.Parse(upgrades[i, 1]) > 0) {
                arrowAmount = int.Parse(upgrades[i, 1]) + 1;
                bowStrength /= 1.5f;
            }
            else if(upgrades[i, 0] == "Piercing" && int.Parse(upgrades[i, 1]) > 0) {
                pierceAmount += int.Parse(upgrades[i, 1]);
            }
            else if(upgrades[i, 0] == "Flaming" && int.Parse(upgrades[i, 1]) > 0){
                flame = true;
                flameLength = 3 + int.Parse(upgrades[i, 1]);
            }
            else if(upgrades[i,0] == "Arrow Speed" && int.Parse(upgrades[i, 1]) > 0){
                speedMult = 1.5f * float.Parse(upgrades[i, 1]);
            }
            else if(upgrades[i,0] == "Damage" && int.Parse(upgrades[i,1]) > 0){
                bowStrength *= 1.25f * float.Parse(upgrades[i, 1]);
            }

        }

        // Spawns arrow
        Vector3 pos = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
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
                prefab.GetComponent<Arrow>().arrowSpeed = arrowSpeed * speedMult;
                prefab.GetComponent<Arrow>().bowStrength = bowStrength;
                prefab.GetComponent<Arrow>().exploding = exploding;
                prefab.GetComponent<Arrow>().flame = flame;
                prefab.GetComponent<Arrow>().flameLength = flameLength;
                Instantiate(prefab, pos, transform.rotation);
                spacing += multishotAngle;
            }
        }
        else {
            prefab.GetComponent<Arrow>().multishotAngle = 0;
            prefab.GetComponent<Arrow>().pierceAmount = pierceAmount;
            prefab.GetComponent<Arrow>().arrowSpeed = arrowSpeed * speedMult;
            prefab.GetComponent<Arrow>().bowStrength = bowStrength;
            prefab.GetComponent<Arrow>().exploding = exploding;
            prefab.GetComponent<Arrow>().flame = flame;
            prefab.GetComponent<Arrow>().flameLength = flameLength;
            Instantiate(prefab, pos, transform.rotation);
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
