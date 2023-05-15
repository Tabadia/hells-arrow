using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

/*
Marks large circle on ground, charges for a few seconds, then does massive damage to player if they are within it
Cannot be parried
Orbits around player slowly enough to shoot with some lag
High health & large
*/

public class GreaterAngel : MonoBehaviour
{
    [SerializeField] private GameObject circle;
    [SerializeField] private GameObject player;
    [SerializeField] private float shootCooldown = 5f;
    [SerializeField] private float shootRange = 300f;
    [SerializeField] private float moveRange = 750f;
    [SerializeField] private AudioSource shootSFX;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject billboard;
    [SerializeField] public NavMeshAgent agent;

    private CapsuleCollider playerCollider;
    private bool canShoot = false;
    private bool canMove = true;
    private float distance;
    private Vector3 healthPos;
    private Collider circleCollider;

    void Start() {
        circleCollider = circle.GetComponent<Collider>();
        playerCollider = player.GetComponent<CapsuleCollider>();
        canShoot = true;
        healthPos = healthBar.transform.position;
        circle.SetActive(false);
    }

    void Update() {
        distance = (transform.position - player.transform.position).sqrMagnitude;

        if (canShoot && distance < shootRange) {
            StartCoroutine(Shoot());
        }

        if ((distance < moveRange) && (distance > shootRange) && canMove) {
            agent.destination = player.transform.position;       
        }
        else {
            agent.destination = transform.position;
        }

        if(transform.position.x > player.transform.position.x){
            billboard.transform.localScale = new Vector3(1, 1, 1);
        } else {
            billboard.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    IEnumerator Shoot() {
        animator.Play("attack",0);
        print("Shoot");
        canShoot = false;
        yield return new WaitForSeconds(1f);
        
        circle.transform.position = player.transform.position;
        circle.SetActive(true);

        yield return new WaitForSeconds(.9f);
        Collider[] hitColliders = Physics.OverlapSphere(circle.transform.position, 4.3f);
        foreach (var hitCollider in hitColliders) {
            if (hitCollider == playerCollider) {
                player.GetComponent<Hearts>().takeDamage(1);
            }
        }
        shootSFX.Play();
        Quaternion rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);

        yield return new WaitForSeconds(.5f);
        circle.SetActive(false);
        yield return new WaitForSeconds(shootCooldown-.5f);
        canShoot = true;
    }
}