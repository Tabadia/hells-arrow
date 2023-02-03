using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int cooldown = 1;
    public float maxCharge = 1f;
    public int despawnTime = 10;
    public int arrowAmount = 1;
    public float bowStrength = 10f;
    private float timer;
    private bool cooldownActive;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            timer = Time.time;
            if(timer > maxCharge)
            {
                if (!cooldownActive) Shoot(timer);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!cooldownActive) Shoot(Time.time - timer);
        }
    }

    private void Shoot(float chargeTime)
    {
        print(chargeTime);

        bowStrength *= chargeTime;

        if (bowStrength < 1) { bowStrength = 1; }
        else if (bowStrength > 10) { bowStrength = 10; }

        print("Strength: " + bowStrength);

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        cooldownActive = true;
        yield return new WaitForSeconds(cooldown);
        cooldownActive = false;
    }
}
