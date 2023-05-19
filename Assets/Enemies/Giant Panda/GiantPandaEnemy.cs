using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class GiantPandaEnemy : MonoBehaviour
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
    private bool isLookingAtPlayer = false;

    [SerializeField] private float sightRange = 500f;
    [SerializeField] private float damageDealt;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float speed = 0.3f;
    [SerializeField] private float knockback = 5f;
    private GameObject player;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject billboard;
    [FormerlySerializedAs("collider")] [SerializeField] private CapsuleCollider Collider;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHearts = player.GetComponent<Hearts>();
        playerCollider = player.GetComponent<CapsuleCollider>();
        canCharge = true;
    }

    void Update() {
        distance = (transform.position - player.transform.position).sqrMagnitude;

        if (distance < sightRange) {
            inSightRange = true;

            if (!isCharging) {
                targetPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

                moveDirection = (targetPos - transform.position).normalized;

                isLookingAtPlayer = false;
                RaycastHit hit;
                for (float i = -1; i <= 1; i += 0.25f) {
                    Debug.DrawRay(new Vector3(transform.position.x, player.transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 5f * i, 0)) * transform.forward * sightRange / 2f, Color.black);
                    Physics.Raycast(
                        new Ray(new Vector3(transform.position.x, player.transform.position.y, transform.position.z),
                            Quaternion.Euler(new Vector3(0, 5f * i, 0)) * transform.forward), out hit, sightRange);
                    if (hit.point != Vector3.zero && hit.collider.gameObject.CompareTag("Player")) {
                        isLookingAtPlayer = true;
                        break;
                    }
                }

                float chargeDistance = Vector3.Distance(transform.position, targetPos);
                totalTime = chargeDistance / speed;
                startTime = Time.time;
            }

            if (!isLookingAtPlayer) {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection, Vector3.up), .25f);
            }

            if (canCharge && isLookingAtPlayer) {
                isCharging = true;
                //Debug.Log("set charging");
                //Animating roll
                animator.SetBool("IsCharging", true);
                float time = (Time.time - startTime) / totalTime;
                transform.position = Vector3.Slerp(transform.position, targetPos, time);
            }

            if (Vector3.Distance(transform.position, targetPos) < 0.5f && isCharging) {
                StartCoroutine(Cooldown());

            }
        }
        else
        {
            inSightRange = false;
        }
        
        if(transform.position.x > player.transform.position.x){
            billboard.transform.localScale = new Vector3(1, 1, 1);
        } else {
            billboard.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Knockback());
        }
    }

    IEnumerator Cooldown()
    {
        animator.SetBool("IsCharging", false);
        isCharging = false;
        canCharge = false;
        yield return new WaitForSeconds(dashCooldown);
        canCharge = true;
    }

    IEnumerator Knockback()
    {
        playerHearts.TakeDamage(0.5f);

        // Player knockback code here
        Vector3 playerPos = player.transform.position;
        Vector3 dir = new Vector3((playerPos - transform.position).x, 0f, (playerPos - transform.position).z).normalized;
        Rigidbody playerRB = player.GetComponent<Rigidbody>();
        MovementController playerMS = player.GetComponent<MovementController>();
        playerRB.velocity = dir * knockback;

        playerMS.preKnockbackMaxSpeed = playerMS.maxSpeed;
        playerMS.maxSpeed *= knockback;
        playerMS.isKnockedBack = true;
        playerMS.knockbackTime = Time.time;
        yield return new WaitForSeconds(0.25f);
        playerMS.maxSpeed /= knockback;
        playerMS.isKnockedBack = false;

        isCharging = false;
        canCharge = false;
        yield return new WaitForSeconds(dashCooldown - 0.25f > 0 ? dashCooldown - 0.25f : 0);
        canCharge = true;
    }
}
