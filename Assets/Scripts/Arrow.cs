using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private int despawnTime = 20;
    public float arrowSpeed;
    private float timer;
    private Vector3 endPos;
    private Vector3 worldPosition;
    private Vector3 moveVector;
    private Vector3 moveDirection;

    void Start()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit))
        {
            endPos = hit.point;
            endPos.y += 1;
        }
        moveDirection = (endPos - transform.position).normalized;
        transform.LookAt(endPos);
        timer = Time.time;
        print("Arrow speed: " + arrowSpeed);
    }

    void Update()
    {
        if (Time.time - timer > despawnTime)
        {
            Destroy(gameObject);
        }
        transform.position += moveDirection * arrowSpeed * Time.deltaTime;

        // IF HITTING WALL OR ENEMY OR ANYTHING, STOP MOVEMENT (will add when maps are actually made)
    }
}
