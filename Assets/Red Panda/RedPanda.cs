using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPanda : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private float shootCooldown = 5f;
    [SerializeField] private float shootRange = 225f;
    [SerializeField] private AudioSource shootSFX;
    [SerializeField] private GameObject healthBar;

    private CapsuleCollider playerCollider;
    private bool canShoot = false;
    private float distance;
    private Animator animator;
    private Vector3 healthPos;

    void Start() {
        playerCollider = player.GetComponent<CapsuleCollider>();
        canShoot = true;
        animator = GetComponent<Animator>();
        healthPos = healthBar.transform.position;
    }

    void Update() {
        distance = (transform.position - player.transform.position).sqrMagnitude;
        if (canShoot && distance < shootRange) {
            animator.SetTrigger("Shoot");
            healthBar.transform.position = new Vector3(transform.position.x, healthPos.y+1, transform.position.z);
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot() {
        print("Shoot");
        canShoot = false;
        yield return new WaitForSeconds(1f);
        shootSFX.Play();
        Quaternion rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        Instantiate(fireballPrefab, transform.position, rotation);
        healthBar.transform.position = new Vector3(transform.position.x, healthPos.y, transform.position.z);
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}

