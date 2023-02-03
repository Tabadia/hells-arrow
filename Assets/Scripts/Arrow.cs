using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public float arrowSpeed = 50f;
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
        }
    }

    void Update()
    {
        float step = arrowSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPos, step);
    }
}
