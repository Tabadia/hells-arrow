using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private int despawnTime = 20;
    public float arrowSpeed = 50f;
    private float timer;
    private Vector3 endPos;
    public Vector3 worldPosition;

    void Start()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            endPos = hit.point;
            endPos.y += 1;
        }
        transform.LookAt(endPos);
        timer = Time.time;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            hitPoint = hit.point;
        }
    }

    void Update()
    {
        if (Time.time - timer > despawnTime)
        {
            Destroy(gameObject);
        }
        float step = arrowSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPos, step);
    }
}
