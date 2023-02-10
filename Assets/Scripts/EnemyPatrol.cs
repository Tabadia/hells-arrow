using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] points;
    public bool isPatrolling = true;
    private Charging charging;
    private int current;
    private float totalTime;

    [SerializeField] private float speed;

    void Start()
    {
        current = 0;
        charging = transform.GetComponent<Charging>();
    }

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
            }
            else {
                current = (current + 1) % points.Length;
            }
        }
        

    }
}
