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

    void Start()
    {

    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (!cooldownActive) timer = Time.time;
            else timer = 0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!cooldownActive) Shoot(Time.time - timer);
            else timer = 0;
        }
    }

    private void Shoot(float chargeTime)
    {
        if (chargeTime > maxCharge)
        {
            chargeTime = 1;
        }
        float bowStrength = defaultBowStrength;
        float arrowSpeed = defaultArrowSpeed;

        arrowSpeed *= chargeTime;
        bowStrength *= chargeTime;

        if (bowStrength < 1) { bowStrength = 1; }
        else if (bowStrength > 10) { bowStrength = 10; }
        if (arrowSpeed < 20) { arrowSpeed = 20; }
        else if (arrowSpeed > 200) { arrowSpeed = 200; }

        print("Strength: " + bowStrength + " Speed: " + arrowSpeed);

        //SHOOT STUFF

        //create gameobject, send it towards mouse
        Instantiate(prefab, transform.position, transform.rotation);
        prefab.GetComponent<Arrow>().arrowSpeed = arrowSpeed;
        //zoom camera towards cursor pos

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        cooldownActive = true;
        yield return new WaitForSeconds(cooldown);
        cooldownActive = false;
    }
}
