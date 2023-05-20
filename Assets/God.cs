using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour
{
    [SerializeField] private GameObject lightning;
    [SerializeField] private GameObject reverseLightning;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float range;
    private Hearts playerHearts;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(LightningStrikes());
        playerHearts = player.GetComponent<Hearts>();
        StartCoroutine(EnemySpawner());
    }

    void Update() {
        if (Vector3.Distance(player.position, transform.position) < range)
        {
            transform.LookAt(player);
        }
    }

    IEnumerator LightningStrikes() {
        while (true)
        {
            
                yield return new WaitForSeconds(Random.Range(2, 3));
                if (Vector3.Distance(player.position, transform.position) < range) {
                    StartCoroutine(Strike());
                }
                yield return new WaitForSeconds(0.2f);
                if (Vector3.Distance(player.position, transform.position) < range) {
                    StartCoroutine(Strike());
                }
        }
    }

    IEnumerator Strike() {
        Vector3 position = player.position + new Vector3(Random.Range(-7.2f, 7.2f), 0, Random.Range(-7.2f, 7.2f));
        GameObject reverse = Instantiate(reverseLightning, position - new Vector3(0, 1f, 0), Quaternion.Euler(90, 0, 0));
        yield return new WaitForSeconds(.5f);
        GameObject g = Instantiate(lightning, position + new Vector3(0, 12.46f, 0), Quaternion.identity);
        if (Vector3.Distance(player.position, position) < 3f)
        {
            playerHearts.TakeDamage(.5f);
        }
        yield return new WaitForSeconds(.5f);
        Destroy(reverse);
        Destroy(g);
    }

    IEnumerator EnemySpawner(){
        while (true) {
            Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position + new Vector3(0, -8, 0), Quaternion.identity);
            yield return new WaitForSeconds(7f);
        }
    }
}