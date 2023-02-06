using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Arrow : MonoBehaviour
{
    // wayyyy tooooo many variables
    [SerializeField] private GameObject player;
    [SerializeField] private int despawnTime = 20;

    public float arrowSpeed;
    public int multiShotArrow;
    public float multishotAngle;
    public int pierceAmount = 0;
    public float bowStrength;

    private float timer;
    private bool colliding = false;
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
                        else {colliding = true;
                        transform.position = hits[i].point;
                        }
                    }
                    else {
                        colliding = true;
                        transform.position = hits[i].point;
                    }
                }
                // if(!pastHits.Contains(hits[i].collider.gameObject.GetInstanceID())){
                //     print("hasnt hit yet");
                //     pastHits[i] = hits[i].collider.gameObject.GetInstanceID();
                //     print("hit enemy");
                //     if (timesPierced < pierceAmount){
                //         print("pierced");
                //         for (int j = 0; j < pastHits.Length; j++){
                //             print(pastHits[j]);
                //         }
                //         timesPierced++;
                //     }
                //     else { print("duplicate");}
                //     }
                //     else {
                //         print("pierce amount: " + pierceAmount + " times pierced: " + timesPierced);
                //         colliding = true;
                //     }
            }
        }
    }
}
