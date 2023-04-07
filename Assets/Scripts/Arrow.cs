using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private int despawnTime = 20;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource shootSFX;

    public float arrowSpeed;
    public int multiShotArrow;
    public float multishotAngle;
    public int pierceAmount = 0;
    public float bowStrength;
    public bool exploding;
    public bool flame;
    public int flameLength;
    private EnemyHealth enemyHealth;
    private float timer;
    private bool colliding = false;
    private bool exploded = false;
    private Vector3 endPos;
    private Vector3 worldPosition;
    private Vector3 moveVector;
    private Vector3 moveDirection;
    private Vector3 prevPos;
    private int timesPierced;

    public int[] pastHits;

    void Start()
    {
        // Converts mouse position to world position
        pastHits = new int[pierceAmount];
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit))
        {
            endPos = hit.point;
            if (endPos.y <= 21) endPos.y = 21;
        }
        // Orients arrow towards mouse position and gets direction for it to go
        moveDirection = (endPos - transform.position).normalized;
        transform.LookAt(endPos);
        moveDirection = Quaternion.Euler(0, multishotAngle, 0) * moveDirection;
        timer = Time.time;
    }

    void Update()
    {
        prevPos = transform.position;
        // Despawn time
        if (Time.time - timer > despawnTime) Destroy(gameObject);

        // Move to direction
        if (!colliding)
        {
            transform.position += moveDirection * arrowSpeed * Time.deltaTime;

            // Raycast stuff
            RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.tag != "Player") {
                    if (hits[i].collider.gameObject.tag == "Enemy") {
                        if (pierceAmount > timesPierced){
                            if(pastHits.Contains(hits[i].collider.gameObject.GetInstanceID())){
                                print("duplicate");
                            }
                            else {
                                print("pierced");
                                OnHit(hits[i]);
                                colliding = false;
                                pastHits[i] = hits[i].collider.gameObject.GetInstanceID();
                                timesPierced++;
                            }
                        }
                        else {
                            OnHit(hits[i]);
                        }
                    }
                    else {
                        OnHit(hits[i]);
                    }
                }
            }
        }
        else if (colliding && !exploded && exploding){
            exploded = true;
            Explode();
        }
    }

    void Explode() {
        Vector3 center = transform.position + transform.forward * explosionRadius/2;
        // Explosion stuff
        Collider[] hitColliders = Physics.OverlapBox(center, new Vector3(explosionRadius / 2, explosionRadius / 2, explosionRadius / 2), Quaternion.Euler(transform.forward));
        foreach(var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Enemy")) {
                enemyHealth = hitCollider.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.takeDamage(bowStrength);
            }
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    void OnHit(RaycastHit hit) {
        colliding = true;
        transform.position = hit.point + transform.forward;
        transform.parent = hit.transform;
        if (hit.collider.gameObject.CompareTag("Enemy"))
        {
            enemyHealth = hit.collider.gameObject.GetComponent<EnemyHealth>();
            /*if(!shootSFX.isPlaying)*/ hitSFX.Play();
            enemyHealth.takeDamage(bowStrength);
            if(flame) {
                StartCoroutine(FireDmg(enemyHealth));
            }
        }
    }

    IEnumerator FireDmg(EnemyHealth enemyHealth)
    {
        for (int i = 0; i < flameLength; i++)
        {
            enemyHealth.takeDamage(1);
            yield return new WaitForSeconds(1f);
        }
        
    }
}
