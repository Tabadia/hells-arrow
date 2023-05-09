using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float damage = .5f;
    
    private GameObject player;
    private Hearts playerHearts;
    private Parry playerParry;
    private CapsuleCollider playerCollider;
    private Vector3 prevPos;

    void Start() {
        player = GameObject.FindWithTag("Player");
        playerHearts = player.GetComponent<Hearts>();
        playerParry = player.GetComponent<Parry>();
        playerCollider = player.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        prevPos = transform.position;
        transform.position += transform.forward * Time.deltaTime * speed;

        RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.tag == "Player")
            {
                if (!playerParry.isParrying)
                {
                    playerHearts.takeDamage(damage);
                }
                else {
                    print("Parried");
                }
            }
            if (hits[i].collider.gameObject.tag != "Arrow"){
                Destroy(gameObject);
            }
        }
    }
}
