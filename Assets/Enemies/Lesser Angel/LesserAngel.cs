using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class LesserAngel : MonoBehaviour
{
[SerializeField] private GameObject beam;
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

    void Start() {
        playerCollider = player.GetComponent<CapsuleCollider>();
        canShoot = true;
        healthPos = healthBar.transform.position;
    }

    void Update() {
        distance = (transform.position - player.transform.position).sqrMagnitude;

        if(canShoot){
            beam.transform.rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        }

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
        beam.SetActive(true);
        yield return new WaitForSeconds(1.9f);
        shootSFX.Play();
        Quaternion rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        healthBar.transform.position = new Vector3(transform.position.x, healthPos.y, transform.position.z);
        yield return new WaitForSeconds(.5f);
        beam.SetActive(false);
        yield return new WaitForSeconds(shootCooldown-.5f);
        canShoot = true;
    }
}