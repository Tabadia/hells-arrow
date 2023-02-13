using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health = 20f;

    void Start()
    {
        
    }

    void Update()
    {
        if (health <= 0){
            Destroy(gameObject);
        }
    }
    
    public void takeDamage(float bowStrength)
    {
        health -= bowStrength;
        print(bowStrength);
    }
}
