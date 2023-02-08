using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementController : MonoBehaviour
{
    // Majority of these are only public so that they can be tweaked on the fly via inspector - should be privatized when dialed in
    [SerializeField] public float moveSpeed = 8.3f;
    public float Gravity = -15f;
    public float maxSpeed = 8.3f;
    public float dashMult = 2.7f;
    public float dashTime = 0.15f;
    public float dashCooldown = 2.0f;
    public float chargingSpeedMultiplier = 5.0f;

    private Vector3 forward, right;
    private Vector3 lastHeading;
    private Vector3 move;
    private Vector3 dashStartSpeed;
    private Vector3 rightMove;
    private Vector3 upMove;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider cc;
    [SerializeField] private PhysicMaterial ppm; // Player Physic Material
    private GroundCheck gcScript;
    private Shrines shrineScript;
    private ShootingScript shootingScript;

    private bool isDashing;
    private bool isJumping;
    private bool isMoveDown;
    private float maxDashCooldown;
    private float verticalInput;
    private float horizontalInput;

    private RaycastHit lastWallHit;
    private Vector3 lastWallHitPos;

    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        maxDashCooldown = dashCooldown;
        shootingScript = GetComponent<ShootingScript>();
        shrineScript = GetComponent<Shrines>();
        gcScript = GetComponent<GroundCheck>();
    }

    void Update()
    {
        horizontalInput = Mathf.Round(Input.GetAxis("Horizontal"));
        verticalInput = Mathf.Round(Input.GetAxis("Vertical"));

        rightMove = horizontalInput * moveSpeed * right;
        upMove = verticalInput * moveSpeed * forward;

        if (dashCooldown < maxDashCooldown)
        {
            dashCooldown += Time.deltaTime;
            dashCooldown = Math.Min(maxDashCooldown, dashCooldown);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldown.Equals(maxDashCooldown))
        {
            isDashing = true;
            Dash();
            dashCooldown = 0f;
        }
        
        isMoveDown = !horizontalInput.Equals(0) || !verticalInput.Equals(0);
        
        Debug.DrawRay(rb.position, rb.velocity, Color.red);
        Debug.DrawRay(lastWallHit.point, Vector3.up, Color.green);
        Debug.DrawRay(lastWallHitPos, (lastWallHit.point - lastWallHitPos).normalized * (lastWallHit.distance - cc.radius*2), Color.blue);
        
        
    }

    private void FixedUpdate()
    {
        if (!isMoveDown && gcScript.isGrounded)
        {
            ppm.dynamicFriction = 4.8f;
        }
        else
        {
            ppm.dynamicFriction = 0.0f;
        }

        Vector3 tempHoriz = Vector3.zero;
        if (shootingScript.isCharging) // Where isCharging is a public value determining if the player is charging their shot
        {  // Movement when Charging
            tempHoriz = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            tempHoriz = Vector3.ClampMagnitude(tempHoriz + rightMove + upMove, maxSpeed / chargingSpeedMultiplier);
        }
        else if (!shrineScript.inMenu)
        {  // Regular Movement
            tempHoriz = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            tempHoriz = Vector3.ClampMagnitude(tempHoriz + rightMove + upMove, maxSpeed);
        }
        
        rb.velocity = tempHoriz;

        if (!gcScript.isGrounded)
        {
            List<RaycastHit> rayHits = new List<RaycastHit>();
            rayHits.Add(new RaycastHit());
            foreach (Vector3 probePosition in gcScript.probePositions)
            {
                RaycastHit hit;
                Physics.Raycast(new Ray(probePosition, Vector3.down), out hit, Mathf.Infinity, gcScript.collisionMask);
                rayHits.Add(hit);
            }

            float highestDist = 0f;
            int indexHighest = 0;
            for (int i = 0; i < gcScript.probePositions.Length; i++)
            {
                if (rayHits[i].distance > highestDist)
                {
                    highestDist = rayHits[i].distance;
                    indexHighest = i;
                }
            }
            
            RaycastHit highestHit = rayHits[indexHighest];
            if (highestHit.point != Vector3.zero)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.MovePosition(new Vector3(rb.position.x, highestHit.point.y+cc.height/2, rb.position.z));
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, Gravity, rb.velocity.z);
            }
        }
        
        RaycastHit moveHit = MoveCheck();
        if (moveHit.point != Vector3.zero)
        {
            dashStartSpeed = Vector3.zero;
                
            lastWallHit = moveHit;
            lastWallHitPos = rb.position;
            rb.velocity = Vector3.zero;
            var distance = moveHit.distance - cc.radius;
            var dir = (rb.position - moveHit.point);
            dir.y = 0;
            
            transform.position += new Vector3(rb.velocity.x, 0f, rb.velocity.z).normalized * distance;
        }
    }

    void Dash()
    {
        if (!IsInvoking(nameof(CancelDash)))
        {
            dashStartSpeed = rb.velocity;
            Vector3 dashNormalSpeed = dashStartSpeed.normalized;
            maxSpeed *= dashMult;
            rb.velocity = new Vector3((dashNormalSpeed.x) * maxSpeed, rb.velocity.y, (dashNormalSpeed.z) * maxSpeed);

            Invoke(nameof(CancelDash), dashTime);
        }
    }
    void CancelDash()
    {
        rb.velocity = new Vector3(dashStartSpeed.x, rb.velocity.y, dashStartSpeed.z);
        // rb.velocity = Vector3.zero;
        dashStartSpeed = Vector3.zero;
        maxSpeed /= dashMult;
        isDashing = false;
    }

    RaycastHit MoveCheck()
    {
        Vector3 velocDirection = new Vector3(rb.velocity.x, 0f, rb.velocity.z).normalized;
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, velocDirection), out hit, rb.velocity.magnitude*Time.deltaTime*1.2f))
        {
            return hit;
        }

        return new RaycastHit();
    }
}


