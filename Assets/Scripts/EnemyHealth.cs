using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {
    [SerializeField] private float maxHealth = 20f;
    [SerializeField] private float health = 20f;
    [SerializeField] public float difficulty = 1f; // 1 for basic enemy, 3(?) for bosses
    [SerializeField] private GameObject enemyDrop;
    [SerializeField] private bool isGiantPanda;
    [SerializeField] private bool isRedPanda;
    [SerializeField] private bool isOni;

    private Slider healthBar;

    void Start() {
        healthBar = GetComponentInChildren<Slider>();
        health = maxHealth;
        healthBar.value = CalculateHealth();
    }

    void Update() {
        healthBar.value = CalculateHealth();
        if (health <= 0){
            enemyDrop.GetComponent<ExperienceScript>().parentDifficulty = difficulty;
            Instantiate(enemyDrop, transform.position+(new Vector3(0,0.5f,0)), transform.rotation);
            Destroy(gameObject);
        }
        if (health > maxHealth){
            health = maxHealth;
        }

        Camera camera = Camera.main;
 
        healthBar.transform.LookAt(healthBar.transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
    }
    
    public void takeDamage(float bowStrength) {
        health -= bowStrength;
        if(isRedPanda)
            gameObject.transform.GetChild(1).GetComponent<Animator>().SetTrigger("Hurt");
        if (isGiantPanda)
        {
            gameObject.transform.GetChild(1).GetComponent<Animator>().Play("GiantPandaHit", 0);
        }
        if(isOni){
            gameObject.transform.GetChild(1).GetComponent<Animator>().SetTrigger("Hurt");
        }
    }

    private float CalculateHealth() {
        return health / maxHealth;
    }
}
