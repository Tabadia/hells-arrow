using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // wayyyy tooooo many variables
    [SerializeField] private GameObject player;
    [SerializeField] private int despawnTime = 20;
    [SerializeField] private float multishotAngle = 5;
    public float arrowSpeed;
    public int multiShotArrow;
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
        if (multiShotArrow == 1)
        {
            moveDirection = Quaternion.Euler(0, 0, 0) * moveDirection;
        }
        else if (multiShotArrow == 2)
        {
            moveDirection = Quaternion.Euler(0, -multishotAngle, 0) * moveDirection;
        }
        else if (multiShotArrow == 3)
        {
            moveDirection = Quaternion.Euler(0, multishotAngle, 0) * moveDirection;
        }
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
