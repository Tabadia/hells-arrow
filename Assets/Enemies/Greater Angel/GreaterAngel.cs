using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GreaterAngel : MonoBehaviour
{
    [SerializeField] private GameObject circle;
    private GameObject player;
    [SerializeField] private float shootCooldown = 5f;
    [SerializeField] private float shootRange = 300f;
    [SerializeField] private float moveRange = 750f;
    [SerializeField] private AudioSource shootSFX;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject billboard;
    [SerializeField] public NavMeshAgent agent;

    private CapsuleCollider playerCollider;
    private bool canShoot = false;
    private bool canMove = true;
    private float distance;
    private Vector3 healthPos;
    private Collider circleCollider;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        circleCollider = circle.GetComponent<Collider>();
        playerCollider = player.GetComponent<CapsuleCollider>();
        canShoot = true;
        healthPos = healthBar.transform.position;
        circle.SetActive(false);
    }

    void Update() {
        distance = (transform.position - player.transform.position).sqrMagnitude;
        
        if (canShoot && distance < shootRange) {
            StartCoroutine(Shoot());
        }

        if ((distance < moveRange) && (distance > shootRange) && canMove) {
            agent.destination = player.transform.position;       
        }
        else {
            agent.destination = transform.position;
        }

        if(transform.position.x > player.transform.position.x){
            billboard.transform.localScale = new Vector3(1, 1, 1);
        } else {
            billboard.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    IEnumerator Shoot() {
        animator.Play("attack",0);
        print("Shoot");
        canShoot = false;
        yield return new WaitForSeconds(1f);
        
        circle.transform.position = player.transform.position;
        circle.SetActive(true);

        yield return new WaitForSeconds(.4f);
        shootSFX.Play();
        Quaternion rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        bool hit = false;
        for (int i = 0; i < 5; i++)
        {
            Collider[] hitColliders = new Collider[100];
            var hitNum = Physics.OverlapSphereNonAlloc(circle.transform.position, 4.3f, hitColliders);
            for (var j = 0; j < hitNum; j++)
            {
                if (hitColliders[i] == playerCollider)
                {
                    if (!hit)
                    {
                        hit = true;
                    }
                }
            }
            
            yield return new WaitForSeconds(.1f);
        }
        if (hit){
            player.GetComponent<Hearts>().TakeDamage(1f);
        }

        yield return new WaitForSeconds(.5f);
        circle.SetActive(false);
        yield return new WaitForSeconds(shootCooldown-.5f);
        canShoot = true;
    }
}