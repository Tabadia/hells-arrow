using System.Collections;
using UnityEngine;

public class Charging : MonoBehaviour
{
    private Hearts playerHearts;
    private bool inRange;
    private Vector3 moveDirection;
    private float distance;
    private Vector3 targetPos;
    private CapsuleCollider playerCollider;
    private float startTime;
    private float totalTime;
    private bool isCharging = false;
    private bool canCharge = true;
    // for patrolling script
    public bool inSightRange = false;

    [SerializeField] private float sightRange = 500f;
    [SerializeField] private float damageDealt;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float knockback = 5f;
    [SerializeField] private GameObject player;
    [SerializeField] private CapsuleCollider collider;
    void Start()
    {
        playerHearts = player.GetComponent<Hearts>();
        playerCollider = player.GetComponent<CapsuleCollider>();
        canCharge = true;
    }

    void Update()
    {
        distance = (transform.position - player.transform.position).sqrMagnitude;
        

        if (distance < sightRange) {
            inSightRange = true;

            if (!isCharging) {
                targetPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                transform.LookAt(targetPos);
                // transform.LookAt(player.transform.position);
                // targetPos = player.transform.position;
                moveDirection = (targetPos - transform.position).normalized;
                float chargeDistance = Vector3.Distance(transform.position, targetPos);
                totalTime = chargeDistance/speed;
                startTime = Time.time;
            } 
            if (canCharge) {
                isCharging = true;
                // Debug.Log(time);
                float time = (Time.time - startTime) / totalTime;
                transform.position = Vector3.Slerp(transform.position, targetPos, time);
                // transform.position += moveDirection * speed * Time.deltaTime;
            }
            // Debug.Log(Vector3.Distance(transform.position, targetPos));
            // Debug.Log(transform.position);
            // Debug.Log(targetPos);
            if (Vector3.Distance(transform.position, targetPos) < 0.5f && isCharging) {
                StartCoroutine(Cooldown());
                
            }
        } else {
            inSightRange = false;
        }
        
    }
    void OnCollisionEnter(Collision col) 
    {
        if (col.gameObject.CompareTag("Player"))
        {    StartCoroutine(Knockback());
        }
    }

    IEnumerator Cooldown() {
        
        isCharging = false;
        canCharge = false;
        yield return new WaitForSeconds(dashCooldown);   
        canCharge = true;
    }

    IEnumerator Knockback() {
        playerHearts.takeDamage(0.5f);
        
        // Player knockback code here
        Vector3 playerPos = player.transform.position;
        Vector3 dir = new Vector3((playerPos - transform.position).x, 0f, (playerPos - transform.position).z).normalized;
        Rigidbody playerRB = player.GetComponent<Rigidbody>();
        MovementController playerMS = player.GetComponent<MovementController>();
        playerRB.velocity = dir * knockback;
        playerMS.maxSpeed *= knockback;
        playerMS.isKnockedBack = true;
        
        isCharging = false;
        canCharge = false;
        yield return new WaitForSeconds(0.25f);   
        playerMS.maxSpeed /= knockback;
        playerMS.isKnockedBack = false;
        yield return new WaitForSeconds(dashCooldown-0.25f>0?dashCooldown-0.25f:0);
        canCharge = true;
    }
}
