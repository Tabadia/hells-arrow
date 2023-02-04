using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // wayyyy tooooo many variables
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
        // Converts mouse position to world position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit))
        {
            endPos = hit.point;
            endPos.y += 1;
        }
        // Orients arrow towards mouse position and gets direction for it to go
        moveDirection = (endPos - transform.position).normalized;
        transform.LookAt(endPos);
        timer = Time.time;
    }

    void Update()
    {
        // Despawn time
        if (Time.time - timer > despawnTime)
        {
            Destroy(gameObject);
        }
        // Move to direction
        transform.position += moveDirection * arrowSpeed * Time.deltaTime;

        // To do: IF HITTING WALL OR ENEMY OR ANYTHING, STOP MOVEMENT (will add when maps are actually made)
    }
}
