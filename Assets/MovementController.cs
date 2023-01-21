using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementController : MonoBehaviour
{
   [SerializeField] public float moveSpeed = 8.3f;
   public float Gravity = -9.8f;
   public float jumpHeight = 5;
   public float maxSpeed = 8.3f;
   public float dashMult = 3.0f;
   public float dashTime = 2.0f;
   public float dashCooldown = 2.0f;

   private Vector3 forward, right;
   private Vector3 lastHeading;
   private Vector3 move;
   private Vector3 dashStartSpeed;

   [SerializeField] private Rigidbody rb;
   [SerializeField] private CapsuleCollider cc;
   
   private bool groundedPlayer;
   private bool isDashing;
   private bool isJumping;
   private bool isMoveDown;
   private float maxDashCooldown;
   private float verticalInput;
   private float horizontalInput;

   void Start()
   {
      forward = Camera.main.transform.forward;
      forward.y = 0;
      forward = Vector3.Normalize(forward);
      right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
      maxDashCooldown = dashCooldown;
   }

   void Update()
   {
      CheckIsGrounded();
      
      horizontalInput = Mathf.Round(Input.GetAxis("Horizontal"));
      verticalInput = Mathf.Round(Input.GetAxis("Vertical"));
      
      Vector3 rightMovement = horizontalInput * moveSpeed * right;
      Vector3 upMovement = verticalInput * moveSpeed * forward;
      move = rightMovement + upMovement;

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
      if (Input.GetButtonDown("Jump") && groundedPlayer)
      {
         isJumping = true;
      }


      if (!isMoveDown) isMoveDown = Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal");
   }

   private void FixedUpdate()
   {
      Vector3 vert = new Vector3(0.0f, 0.0f, 0.0f);
      if (groundedPlayer && vert.y < 0)
      {
         vert.y = 0f;
      }

      if (isJumping)
      {
         vert.y = Mathf.Sqrt(jumpHeight * 12.0f * -Gravity);
         if (groundedPlayer)
         {
            isJumping = false;
         }
      }

      if (isMoveDown) // movement tech: just press the opposite key to get insta max speed
      { // TODO: fix that lmao
         isMoveDown = false;
         rb.AddForce(5f * move, ForceMode.Impulse);
      }
      else
      {
         rb.AddForce(move * 2f); 
      }
      
      rb.AddForce(vert, ForceMode.Impulse);
      Vector3 tempHoriz = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
      Vector3 tempVert = new Vector3(0.0f, rb.velocity.y, 0.0f);
      tempHoriz = Vector3.ClampMagnitude(tempHoriz, maxSpeed);
      rb.velocity = tempHoriz + tempVert;


      Vector3 look = new Vector3(move.x, 0.0f, move.z);
      if (look.magnitude > 0.01)
      {
         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), 0.15F);
      }
   }

   void Dash() // TODO: this should be an absolute dash in the input direction, not multiplying current speed+direction
   {
      if (!IsInvoking(nameof(CancelDash)))
      {
         dashStartSpeed = rb.velocity;
         maxSpeed *= dashMult;
         rb.velocity = new Vector3((dashStartSpeed.x) * dashMult, rb.velocity.y, (dashStartSpeed.z) * dashMult);
         
         // rb.velocity = new Vector3(horizontalInput * dashMult * maxSpeed, rb.velocity.y, verticalInput * dashMult * maxSpeed);
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



   void CheckIsGrounded() {
      //Raycast method
      // groundedPlayer = Physics.Raycast(gameObject.transform.position, -Vector3.up, distToGround+0.05f);
      
      //Spherecast method
      Vector3 pos = rb.position - new Vector3(0, cc.height/2, 0);
      groundedPlayer = Physics.CheckSphere(pos, 0.1f, LayerMask.GetMask("Ground"));
   }
}
   

