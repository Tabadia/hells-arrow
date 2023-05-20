using System.Collections;
using UnityEngine;

public class God : MonoBehaviour
{
    [SerializeField] private GameObject lightning;
    [SerializeField] private GameObject reverseLightning;
    private Transform playerTransform;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float range;
    private Hearts playerHearts;

    // Update is called once per frame
    void Start() {
        playerTransform = player.transform;
        StartCoroutine(LightningStrikes());
        playerHearts = playerTransform.GetComponent<Hearts>();
        StartCoroutine(EnemySpawner());
    }

    void Update() {
        if (Vector3.Distance(playerTransform.position, transform.position) < range) {
            transform.LookAt(playerTransform);
        }
    }

    IEnumerator LightningStrikes() {
        while (true) {
            if (Vector3.Distance(player.transform.position, transform.position) < 60)
            {
                yield return new WaitForSeconds(Random.Range(3, 4));
                if (Vector3.Distance(playerTransform.position, transform.position) < range)
                {
                    StartCoroutine(Strike());
                    yield return new WaitForSeconds(.2f);
                    StartCoroutine(Strike());
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Strike() {
        Vector3 position = playerTransform.position + new Vector3(Random.Range(-7.2f, 7.2f), 0, Random.Range(-7.2f, 7.2f));
        GameObject reverse = Instantiate(reverseLightning, position - new Vector3(0, 1f, 0), Quaternion.Euler(90, 0, 0));
        yield return new WaitForSeconds(.5f);
        GameObject g = Instantiate(lightning, position + new Vector3(0, 12.46f, 0), Quaternion.identity);
        if (Vector3.Distance(playerTransform.position, position) < 3f)
        {
            playerHearts.TakeDamage(.5f);
        }
        yield return new WaitForSeconds(.5f);
        Destroy(reverse);
        Destroy(g);
    }

    IEnumerator EnemySpawner(){
        while (true) {
            if (Vector3.Distance(player.transform.position, transform.position) < range){
                Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position + new Vector3(0, -8, 0), Quaternion.identity);
                yield return new WaitForSeconds(15f);
            }
             yield return new WaitForEndOfFrame();
        }
    }
}
