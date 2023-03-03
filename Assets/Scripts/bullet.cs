using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private GameObject player;

    private Vector3 prevPos;
    private Hearts playerHearts;
    void Start()
    {
        //print("Took damage from bullet.cs");
        //playerHearts = player.GetComponent<Hearts>();
        //playerHearts.takeDamage(0.5f);

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
                print("Hit player");
                //playerHearts.takeDamage(0.5f);
            }
            Destroy(gameObject);
        }
    }
}
