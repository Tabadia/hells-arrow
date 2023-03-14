using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float range = 100f;

    private CapsuleCollider playerCollider;
    private bool canShoot = false;
    private float distance;

    void Start()
    {
        playerCollider = player.GetComponent<CapsuleCollider>();
        canShoot = true;
    }

    void Update()
    {
        distance = (transform.position - player.transform.position).sqrMagnitude;
        transform.LookAt(player.transform);

        if (canShoot && distance < range)
        {
            StartCoroutine(Shoot());
        }

    }

    IEnumerator Shoot()
    {
        canShoot = false;
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
