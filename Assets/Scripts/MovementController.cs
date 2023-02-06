using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementController : MonoBehaviour
{
    // Majority of these are only public so that they can be tweaked on the fly via inspector - should be privatized when dialed in
    [SerializeField] public float moveSpeed = 8.3f;
    public float Gravity = -15f;
    public float jumpHeight = 5f;
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
    private Shrines shrineScript;
    private ShootingScript shootingScript;

    private bool isGrounded;
    private bool isDashing;
    private bool isJumping;
    private bool isMoveDown;
    private float maxDashCooldown;
    private float verticalInput;
    private float horizontalInput;
    private float vertMove;

    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        maxDashCooldown = dashCooldown;
        shootingScript = GetComponent<ShootingScript>();
        shrineScript = GetComponent<Shrines>();
    }

    void Update()
    {
        CheckIsGrounded();

        horizontalInput = Mathf.Round(Input.GetAxis("Horizontal"));
        verticalInput = Mathf.Round(Input.GetAxis("Vertical"));

        rightMove = horizontalInput * moveSpeed * right;
        upMove = verticalInput * moveSpeed * forward;
        vertMove = Gravity * Time.deltaTime;

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

        // Removed Jump Code
        // if (Input.GetButtonDown("Jump") && groundedPlayer)
        // {
        //    isJumping = true;
        // }


        isMoveDown = !horizontalInput.Equals(0) || !verticalInput.Equals(0);
    }

    private void FixedUpdate()
    {
        // Removed Jump Code
        // Vector3 vert = new Vector3(0.0f, 0.0f, 0.0f);
        // if (groundedPlayer && vert.y < 0)
        // {
        //    vert.y = 0f;
        // }
        // if (isJumping)
        // {
        //    vert.y = Mathf.Sqrt(jumpHeight * 12.0f * -Gravity);
        //    if (groundedPlayer)
        //    {
        //       isJumping = false;
        //    }
        // }

        if (isMoveDown)
        {
            ppm.dynamicFriction = 0.0f;
        }
        else
        {
            ppm.dynamicFriction = 0.8f;
        }

        Vector3 tempVert = new Vector3(0.0f, rb.velocity.y + (isGrounded ? 0.0f : vertMove), 0.0f);
        Vector3 tempHoriz = Vector3.zero;
        if (shootingScript.isCharging) // Where isCharging is a public value determining if the player is charging their shot
        {  // Regular Movement
            tempHoriz = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            tempHoriz = Vector3.ClampMagnitude(tempHoriz + rightMove + upMove, maxSpeed / chargingSpeedMultiplier);
        }
        else if (!shrineScript.inMenu)
        {
            tempHoriz = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            tempHoriz = Vector3.ClampMagnitude(tempHoriz + rightMove + upMove, maxSpeed);
        }

        rb.velocity = tempHoriz + tempVert;

        // Rotation about the Y axis is the cause of sliding along slope normals - this method will cause issues when
        // we implement facing directions later
        //
        // Vector3 look = new Vector3(move.x, 0.0f, move.z);
        // if (look.magnitude > 0.01)
        // {
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), 0.15F);
        // }
    }

    void Dash() // investigate charlie clipping through walls, although I did not experience it with brief testing
    {
        //if (shootingScript.isCharging) return;

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
        dashStartSpeed = Vector3.zero;
        maxSpeed /= dashMult;
        isDashing = false;
    }



    void CheckIsGrounded()
    {
        //Raycast method
        // groundedPlayer = Physics.Raycast(gameObject.transform.position, -Vector3.up, distToGround+0.05f);

        //Spherecast method
        Vector3 pos = rb.position - new Vector3(0, cc.height / 2, 0);
        isGrounded = Physics.CheckSphere(pos, 0.05f, LayerMask.GetMask("Ground"));
    }
}


