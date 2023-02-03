using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    // way too many variables
    public int cooldown = 1;
    public float maxCharge = 1f;
    public int despawnTime = 10;
    public int arrowAmount = 1;
    public float defaultBowStrength = 10f;
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
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!cooldownActive) Shoot(Time.time - timer);
        }
    }

    private void Shoot(float chargeTime)
    {
        if (chargeTime > maxCharge)
        {
            chargeTime = 1;
        }
        float bowStrength = defaultBowStrength;

        bowStrength *= chargeTime;

        if (bowStrength < 1) { bowStrength = 1; }
        else if (bowStrength > 10) { bowStrength = 10; }

        print("Strength: " + bowStrength);

        Vector3 mousePos = Input.mousePosition;
        print("Pos: " + mousePos.x + ", " + mousePos.y);

        //SHOOT STUFF

        //create gameobject, send it towards mouse
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
