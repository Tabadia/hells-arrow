using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    private CapsuleCollider playerCollider;
    private bool canShoot = false;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private float shootCooldown = 2f;
    void Start()
    {
        playerCollider = player.GetComponent<CapsuleCollider>();
        canShoot = true;
    }

    void Update()
    {
        transform.LookAt(player.transform);
        if (canShoot)
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
