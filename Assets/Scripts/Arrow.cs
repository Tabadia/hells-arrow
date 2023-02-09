using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Arrow : MonoBehaviour
{
    // wayyyy tooooo many variables
    [SerializeField] private GameObject player;
    [SerializeField] private int despawnTime = 20;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private GameObject explosionPrefab;

    public float arrowSpeed;
    public int multiShotArrow;
    public float multishotAngle;
    public int pierceAmount = 0;
    public float bowStrength;
    public bool exploding;

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
                                colliding = false;
                                pastHits[i] = hits[i].collider.gameObject.GetInstanceID();
                                timesPierced++;
                            }
                        }
                        else {
                            colliding = true;
                            transform.position = hits[i].point + transform.forward;
                            transform.parent = hits[i].transform;
                        }
                    }
                    else {
                        colliding = true;
                        transform.position = hits[i].point + transform.forward;
                        transform.parent = hits[i].transform;
                    }
                }
            }
        }
        else if (colliding && !exploded && exploding){
            exploded = true;
            Explode();
        }
    }

    void Explode()
    {
        Vector3 center = transform.position + transform.forward * explosionRadius/2;
        // Explosion stuff
        Collider[] hitColliders = Physics.OverlapBox(center, new Vector3(explosionRadius / 2, explosionRadius / 2, explosionRadius / 2), Quaternion.Euler(transform.forward));
        foreach(var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Enemy"))
            {
                //do damage later but
                Destroy(hitCollider.gameObject);
            }
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
