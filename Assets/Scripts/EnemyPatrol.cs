using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] points;
    public bool isPatrolling = true;
    private Charging charging;
    int current;
    [SerializeField] private float speed;
    [SerializeField] private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        charging = enemy.GetComponent<Charging>();
    }

    // Update is called once per frame
    void Update()
    {   

        if (charging.inSightRange) {
            isPatrolling = false;
        } else {
            isPatrolling = true;
        }

        if (isPatrolling) {
            
            if (transform.position != points[current].position) {  
                transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime); 
            } else {
                current = (current + 1) % points.Length;
            }
        }
        

    }
}
