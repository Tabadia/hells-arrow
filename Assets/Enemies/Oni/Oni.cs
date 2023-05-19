using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Oni : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float atkCooldown = 1f;
    [SerializeField] private float atkRange = 50f;
    [SerializeField] private float sightRange = 500f;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Animator animator;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] private GameObject oniBillboard;
    //[SerializeField] private AudioSource attackSFX;

    private CapsuleCollider playerCollider;
    private Hearts playerHearts;
    private bool canAtk = false;
    private bool canMove = true;
    private float distance;
    private Vector3 healthPos;
    public Vector2 relativePoint;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponent<CapsuleCollider>();
        playerHearts = player.GetComponent<Hearts>();
        canAtk = true;
    }

    void Update() {
        distance = (transform.position - player.transform.position).sqrMagnitude;
        if (distance < atkRange) {
            if (canAtk){
                canMove = false;
                StartCoroutine(Attack());
            }
        }
        else if ((distance < sightRange) && (distance > atkRange) && canMove) {
            agent.destination = player.transform.position;
            if (!(this.animator.GetCurrentAnimatorStateInfo(0).IsName("hurt"))){
                animator.Play("Walk",0);
            }            
        }

        if(transform.position.x > player.transform.position.x){
            oniBillboard.transform.localScale = new Vector3(1, 1, 1);
        } else {
            oniBillboard.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    IEnumerator Attack() {
        canAtk = false;
        //attackSFX.Play();
        animator.Play("Attack",0);
        yield return new WaitForSeconds(.5f);
        playerHearts.takeDamage(1);
        yield return new WaitForSeconds(atkCooldown);
        canAtk = true;
        canMove = true;
    }
}
