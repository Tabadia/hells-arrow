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
    [SerializeField] public float moveSpeed = 4;
    public float Gravity = -15f;
    public float maxSpeed = 16; // This needs to remain public
    public float dashMult = 2.7f;
    public float dashTime = 0.15f;
    public float dashCooldown = 2.0f;
    public float chargingSpeedMultiplier = 5.0f;
    private Vector3 dashStartSpeed;
    [SerializeField] private float rockFriction = 4.8f;
    [SerializeField] private float iceFriction = 1.6f;

    // Essential movement variables
    private Vector3 forward, right;
    private Vector3 lastHeading;
    private Vector3 move;
    private Vector3 rightMove;
    private Vector3 upMove;

    // Scene-related variables
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider cc;
    [SerializeField] private PhysicMaterial ppm; // Player Physic Material
    [SerializeField] private ParticleSystem dashParticleSystem;
    [SerializeField] private GameObject shrinesObject;
    private GroundCheck gcScript;
    private ShrineManager shrineScript;
    private ShootingScript shootingScript;

    // Misc variables / Control related stuff
    private bool isDashing;
    private bool isJumping;
    private bool isMoveDown;
    private float maxDashCooldown;
    private float verticalInput;
    private float horizontalInput;
    private Quaternion cameraLook;
    [NonSerialized] public bool isKnockedBack = false;
    [NonSerialized] public float preKnockbackMaxSpeed;
    [NonSerialized] public float knockbackTime;
    [SerializeField] private AudioSource runSFX;
    [SerializeField] private AudioSource dashSFX;
    [SerializeField] private Animator samuraiAnimator;
    [SerializeField] private GameObject samuraiGameObject;

    // these are technically only for debug - allow the Rays for wall collision checks to be drawn
    private RaycastHit lastWallHit;
    private Vector3 lastWallHitPos;

    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        cameraLook = Camera.main.transform.rotation;
        
        maxDashCooldown = dashCooldown;
        
        shootingScript = GetComponent<ShootingScript>();
        shrineScript = shrinesObject.GetComponent<ShrineManager>();
        gcScript = GetComponent<GroundCheck>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (dashCooldown < maxDashCooldown)
        {
            dashCooldown += Time.deltaTime;
            dashCooldown = Math.Min(maxDashCooldown, dashCooldown);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldown.Equals(maxDashCooldown)) {
            if (!shootingScript.isCharging)
            {
                isDashing = true;
                dashCooldown = 0f;
                Dash();
            }
            else print(shootingScript.isCharging);
        }
        
        isMoveDown = !horizontalInput.Equals(0) || !verticalInput.Equals(0);

        if (isMoveDown) // Player Rotation Control
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.LookRotation(right * horizontalInput + forward * verticalInput, Vector3.up), .25f);

            samuraiAnimator.SetBool("IsRunning", true);
        }
        else
        {
            samuraiAnimator.SetBool("IsRunning", false);
        }
        
        // Debug stuff for movement and wall checks, can be removed anytime but i like having it for now
        Debug.DrawRay(rb.position, rb.velocity, Color.red);
        Debug.DrawRay(lastWallHit.point, Vector3.up, Color.green);
        Debug.DrawRay(lastWallHitPos, (lastWallHit.point - lastWallHitPos).normalized * (lastWallHit.distance - cc.radius*2), Color.blue);
        if(rb.velocity.magnitude <= 0.01 || shootingScript.isCharging){
            runSFX.Stop();
        }
        else {
            if (!runSFX.isPlaying) { runSFX.Play(); }
        }
    }

    private void FixedUpdate()
    {
        if (!isMoveDown && gcScript.isGrounded) // Check if the player is not actively adding movement input and is grounded
        {
            bool isIce = false;   
            foreach (Vector3 probePosition in gcScript.probePositions) // Look through each probe defined in the ground check script
            {
                RaycastHit hit; // Raycast to see what type of terrain player is on
                Physics.Raycast(new Ray(probePosition, Vector3.down), out hit, Mathf.Infinity, gcScript.collisionMask); // already known to be grounded so distance doesnt matter, hence infinite distance 
                if (hit.point != Vector3.zero)
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ice"))
                    {
                        isIce = true;
                    }
                }
            }
        
            if (isIce)
            {
                ppm.dynamicFriction = iceFriction;
            }
            else
            {
                ppm.dynamicFriction = rockFriction;
            }
        }
        else
        {
            ppm.dynamicFriction = 0.0f;
        }

        
        rightMove = horizontalInput * moveSpeed * right;
        upMove = verticalInput * moveSpeed * forward;
        
        Vector3 tempHoriz = Vector3.zero;
        if (shootingScript.isCharging)
        {  // Movement when Charging
            tempHoriz = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            tempHoriz = Vector3.ClampMagnitude(tempHoriz + rightMove + upMove, maxSpeed / chargingSpeedMultiplier);
        }
        else if (!shrineScript.inMenu)
        {  // Regular Movement
            tempHoriz = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            tempHoriz = Vector3.ClampMagnitude(tempHoriz + rightMove + upMove, maxSpeed);
        }

        if (!isKnockedBack)
        {
            rb.velocity = tempHoriz;
        }
        else
        {
            //Debug.Log(Time.time - knockbackTime);
            //Debug.Log(knockbackTime);
            if (Time.time - knockbackTime > .5f)
            {
                isKnockedBack = false;
                maxSpeed = preKnockbackMaxSpeed;
            }
        }

        if (!gcScript.isGrounded) // Logic code to find nearest ground and stick to it
        {
            List<RaycastHit> rayHits = new List<RaycastHit>();
            rayHits.Add(new RaycastHit()); // Initialize a default raycast to (0,0,0) for when none hit
            foreach (Vector3 probePosition in gcScript.probePositions) // Iterate through each probe
            {
                RaycastHit hit;
                Physics.Raycast(new Ray(probePosition, Vector3.down), out hit, Mathf.Infinity, gcScript.collisionMask); // Raycast to the nearest ground object below player
                rayHits.Add(hit);
            }
        
            float highestDist = 0f; // Logic to make sure we don't clip into the ground on slopes
            int indexHighest = 0; // Basically, if multiple probes hit someething, the one with the highest Y value is used
            for (int i = 0; i < gcScript.probePositions.Length; i++)
            {
                if (rayHits[i].distance > highestDist)
                {
                    highestDist = rayHits[i].distance;
                    indexHighest = i;
                }
            }
            
            RaycastHit highestHit = rayHits[indexHighest];
            if (highestHit.point != Vector3.zero) // Filter out default raycast - triggers when no ground is below the player
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reset Y velocity
                rb.MovePosition(new Vector3(rb.position.x, highestHit.point.y+cc.height/2, rb.position.z)); // Move accounting for the player height
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, Gravity, rb.velocity.z); // If no ground is below, just apply gravity and pray
            }
        }
        
        RaycastHit moveHit = MoveCheck(); // Check if the player will collide w/ phase through wall next movement
        if (moveHit.point != Vector3.zero) // Returns default RaycastHit if not, filter those out
        {
            dashStartSpeed = Vector3.zero; // Make sure player doesn't keep going after dash ends
                
            lastWallHit = moveHit; // Debug
            lastWallHitPos = rb.position; // Debug
            rb.velocity = Vector3.zero; // Stop player movement when colliding with wall
            var distance = moveHit.distance - cc.radius; // Account for Player Radius
            
            transform.position += new Vector3(rb.velocity.x, 0f, rb.velocity.z).normalized * distance;
            // Okay so like technically this just teleports you, but dash is so fast that its not worth using Slerp...
        }
    }

    void Dash()
    {
        if (!IsInvoking(nameof(CancelDash))) // Prevent this from being called multiple times
        {
            dashSFX.Play();
            // Emitting particles at the prev location of the player
            dashParticleSystem.Emit(UnityEngine.Random.Range(10, 20));
            dashStartSpeed = rb.velocity;
            Vector3 dashNormalSpeed = dashStartSpeed.normalized;
            maxSpeed *= dashMult;
            rb.velocity = new Vector3((dashNormalSpeed.x) * maxSpeed, rb.velocity.y, (dashNormalSpeed.z) * maxSpeed);

            Invoke(nameof(CancelDash), dashTime); // Call the CancelDash() method after (dashTime) seconds
        }
    }
    void CancelDash()
    {
        rb.velocity = new Vector3(dashStartSpeed.x, rb.velocity.y, dashStartSpeed.z);
        dashStartSpeed = Vector3.zero;
        maxSpeed /= dashMult;
        isDashing = false;
    }
    
    RaycastHit MoveCheck()
    {
        Vector3 velocDirection = new Vector3(rb.velocity.x, 0f, rb.velocity.z).normalized;
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, velocDirection), out hit, rb.velocity.magnitude*Time.deltaTime*1.2f))
        { // Check for walls within the movement change next frame, times 1.2 basically to account for player size
            return hit;
        }
    
        return new RaycastHit(); // If nothing is hit, return a default hit to (0,0,0)
    }
}


