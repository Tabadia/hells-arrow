using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPanda : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    private GameObject player;
    [SerializeField] private float shootCooldown = 5f;
    [SerializeField] private float shootRange = 225f;
    [SerializeField] private AudioSource shootSFX;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject billboard;

    private CapsuleCollider playerCollider;
    private bool canShoot = false;
    private float distance;
    private Vector3 healthPos;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponent<CapsuleCollider>();
        canShoot = true;
        healthPos = healthBar.transform.position;
    }

    void Update() {
        distance = (transform.position - player.transform.position).sqrMagnitude;
        if (canShoot && distance < shootRange) {
            animator.Play("shoot",0);
            healthBar.transform.position = new Vector3(transform.position.x, healthPos.y+1, transform.position.z);
            StartCoroutine(Shoot());
        }

        if(transform.position.x > player.transform.position.x){
            billboard.transform.localScale = new Vector3(1, 1, 1);
        } else {
            billboard.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    IEnumerator Shoot() {
        canShoot = false;
        yield return new WaitForSeconds(.75f);
        shootSFX.Play();
        Quaternion rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        Instantiate(fireballPrefab, transform.position, rotation);
        healthBar.transform.position = new Vector3(transform.position.x, healthPos.y, transform.position.z);
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}

