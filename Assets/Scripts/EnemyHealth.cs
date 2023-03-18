using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 20f;
    [SerializeField] private float health = 20f;
    [SerializeField] private GameObject enemyDrop; //just spawning a thingy for now

    private Slider healthBar;

    void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
        health = maxHealth;
        healthBar.value = CalculateHealth();
    }

    void Update()
    {
        healthBar.value = CalculateHealth();
        if (health <= 0){
            Instantiate(enemyDrop, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if (health > maxHealth){
            health = maxHealth;
        }

        Camera camera = Camera.main;
 
        healthBar.transform.LookAt(healthBar.transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
    }
    
    public void takeDamage(float bowStrength)
    {
        health -= bowStrength;
        //print(bowStrength);
    }

    private float CalculateHealth()
    {
        return health / maxHealth;
    }
}
