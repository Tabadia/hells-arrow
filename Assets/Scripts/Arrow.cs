using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private int despawnTime = 20;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioSource hitSFX;
    // [SerializeField] private AudioSource shootSFX;

    public float arrowSpeed;
    // public int multiShotArrow;
    public float multishotAngle;
    public int pierceAmount;
    public float bowStrength;
    public bool exploding;
    public bool flame;
    public int flameLength;
    private EnemyHealth enemyHealth;
    private float timer;
    private bool colliding;
    private bool exploded;
    private Vector3 endPos;
    private Vector3 worldPosition;
    private Vector3 moveVector;
    private Vector3 moveDirection;
    private Vector3 prevPos;
    private int timesPierced;
    private GameObject player;

    public int[] pastHits;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // Converts mouse position to world position
        pastHits = new int[pierceAmount];
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        if (Physics.Raycast(castPoint, out var hit))
        {
            endPos = hit.point;
            if (endPos.y <= player.transform.position.y) endPos.y = player.transform.position.y+1;
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
            transform.position +=  arrowSpeed * Time.deltaTime * moveDirection;
            var curPos = transform.position;
            
            // Raycast stuff
            var hits = new RaycastHit[pierceAmount+1];
            var hitNum = Physics.RaycastNonAlloc(new Ray(prevPos, (curPos - prevPos).normalized), hits, (curPos - prevPos).magnitude);
            
            for (int i = 0; i < hitNum; i++)
            {
                if (!hits[i].IsUnityNull() && !hits[i].collider.gameObject.CompareTag("Player"))
                {
                    if (hits[i].collider.gameObject.CompareTag("Enemy"))
                    {
                        if (pierceAmount > timesPierced)
                        {
                            if (!pastHits.Contains(hits[i].collider.gameObject.GetInstanceID()))
                            {
                                colliding = false;
                                pastHits[i] = hits[i].collider.gameObject.GetInstanceID();
                                timesPierced++;
                            }
                        }
                        else
                        {
                            OnHit(hits[i]);
                        }
                    }
                    else
                    {
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
        print("explode");
        Vector3 center = transform.position + transform.forward * explosionRadius/2;
        // Explosion stuff
        var hitColliders = new Collider[1000];
        Physics.OverlapBoxNonAlloc(center, new Vector3(explosionRadius / 2, explosionRadius / 2, explosionRadius / 2), hitColliders, Quaternion.Euler(transform.forward));
        foreach(var hitCollider in hitColliders)
        {
            if (hitCollider == null) continue;
            print(hitCollider);
            print(hitCollider.gameObject.name);
            if (hitCollider.CompareTag("Enemy")){
                enemyHealth = hitCollider.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(bowStrength);
                print("hit enemy");
            }
            else {
                print("hit something else");
            }
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    void OnHit(RaycastHit hit) {
        colliding = true;
        transform.position = hit.point + transform.forward;
        transform.parent = hit.transform;
        if (hit.collider.gameObject.CompareTag("Enemy")) {
            enemyHealth = hit.collider.gameObject.GetComponent<EnemyHealth>();
            
            /*if(!shootSFX.isPlaying)*/ hitSFX.Play();
            enemyHealth.TakeDamage(bowStrength);
            if(flame) {
                StartCoroutine(FireDmg(enemyHealth));
            }
        }
    }

    IEnumerator FireDmg(EnemyHealth health)
    {
        for (int i = 0; i < flameLength; i++)
        {
            health.TakeDamage(1, true);
            yield return new WaitForSeconds(1f);
        }
        
    }
}
