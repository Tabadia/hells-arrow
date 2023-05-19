using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1f)] private float length = 0.2f;
    [SerializeField, Range(1, 50)] private int numProbes = 4;
    [SerializeField, Range(0.05f, 1f)] private float probeOffset = 0.5f;
    [SerializeField] private LayerMask[] a = new LayerMask[2];
    [NonSerialized] public LayerMask collisionMask;

    public bool isGrounded;

    private CapsuleCollider capsuleCollider;
    public Vector3[] probePositions;

    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        
        foreach (LayerMask layer in a) // Combine all the given terrain layers into one Mask
        {
            collisionMask = collisionMask | layer; // weird ass bit notation
        }
    }

    void Update()
    {
        probePositions = new Vector3[numProbes];
        Vector3 colliderSize = capsuleCollider.bounds.size;
        Vector3 position = transform.position;
        
        
        // Account 0.05f for the Y so that rays don't start inside the ground and miss
        Vector3 center = new Vector3(position.x, position.y - (colliderSize.y / 2) + 0.05f, position.z);
        
        // 1 and 2 probes are special cases, handle separately
        switch (numProbes) {
            case 1:
                probePositions[0] = center;
                break;
            case 2:
                float distance = Mathf.Sqrt(Mathf.Pow(colliderSize.x / 4, 2) +
                                            Mathf.Pow(colliderSize.z / 4, 2)) * probeOffset;
                probePositions[0] = new Vector3(center.x - distance, center.y,
                    center.z - distance);
                probePositions[1] = new Vector3(center.x + distance, center.y,
                    center.z + distance);
                break;
            default:
                float step = 2*Mathf.PI / (numProbes-1);
                probePositions[0] = center;

                float tan = colliderSize.x * (probeOffset/2);
                for (int i = 1; i < numProbes; i++)
                {
                    float angle = step * i;
                    float sin = Mathf.Sin(angle) * tan;
                    float cos = Mathf.Cos(angle) * tan;
                    probePositions[i] = new Vector3(center.x + sin, center.y, center.z + cos);           
                }
                break;
        }
        
        isGrounded = false;
        foreach (Vector3 probePosition in probePositions)
        {
            if (Physics.Raycast(new Ray(probePosition, Vector3.down), length, collisionMask))
            {
                isGrounded = true;
            }
        }
    }
}