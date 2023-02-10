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
        
    }
    
    public void takeDamage(float bowStrength)
    {
        print(bowStrength);
    }
}
