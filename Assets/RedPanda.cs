using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPanda : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private float shootCooldown = 5f;
    [SerializeField] private float shootRange = 300f;
    [SerializeField] private float followRange = 200f;
    [SerializeField] private AudioSource shootSFX;

    private CapsuleCollider playerCollider;
    private bool canShoot = false;
    private float distance;

    void Start() {
        playerCollider = player.GetComponent<CapsuleCollider>();
        canShoot = true;
    }

    void Update() {
        distance = (transform.position - player.transform.position).sqrMagnitude;
        transform.LookAt(player.transform);

        if (canShoot && distance < shootRange && distance > followRange) {
            StartCoroutine(Wait(2f));
            StartCoroutine(Shoot());
            StartCoroutine(Wait(2f));
        }
        else if (distance < followRange) {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.05f);
        }

    }

    IEnumerator Shoot()
    {
        print("Shoot");
        canShoot = false;
        Instantiate(fireballPrefab, transform.position, transform.rotation);
        shootSFX.Play();
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}

