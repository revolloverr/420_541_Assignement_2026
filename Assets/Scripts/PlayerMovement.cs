using UnityEngine;
using UnityEngine.SceneManagement;

// Moves the player with a Rigidbody: 
// A/D or Left/Right arrows to ROTATE, W/S or Up/Down to move Forward/Backward, Space to charge & jump.
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;          // units per second
    [SerializeField] private float sprintMultiplier = 2f; // multiplier when holding Shift
    [SerializeField] private float turnSpeed = 150f;      // degrees per second

    [Header("Jump Settings")]
    [SerializeField] private float minJumpForce = 2f;     // minimum jump force (tap)
    [SerializeField] private float maxJumpForce = 8f;     // maximum jump force (held for 3s)
    [SerializeField] private float maxChargeTime = 3f;    // time required for max jump force
    [SerializeField] private float doubleJumpForce = 4f;

    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f; // ray length
    [SerializeField] private LayerMask groundMask;        // ground layers

    [SerializeField] private PlayerVisualEffects visualEffects;
    [SerializeField] private AudioClip boingSound;
    private PlayerPowerUps powerUps;

    private Rigidbody rb;               // the physics body we move
    private Vector3 moveDirection;      // worked out from input in Update()
    private bool isGrounded;            // true when standing on the ground
    
    // Jump charging state variables
    private bool isChargingJump = false;
    private float jumpHoldTime = 0f;
    private bool jumpRequested = false;
    private float finalJumpForce = 0f;

    private bool doubleJumpRequested = false; 
    private bool hasDoubleJumped = false;     
    private bool isSprinting = false;   

    public float SpeedMultiplier { get; set; } = 1f;
    public bool IsGrounded => isGrounded;

    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        powerUps = GetComponent<PlayerPowerUps>();
    }

    private void Update()
    {
        // 1. Ground Check
        isGrounded = Physics.Raycast(transform.position + transform.up * groundDistance / 2, -transform.up, groundDistance, groundMask);

        if(transform.position.y < -2){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Back on the ground: the double jump is available again
        if (isGrounded)
        {
            hasDoubleJumped = false;
        }

        // 2. Read Inputs
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right turn input
        float moveZ = Input.GetAxisRaw("Vertical");  // W/S or Up/Down forward/backward

        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        moveDirection = cameraForward * moveZ + cameraRight * moveX;

        if (moveDirection.magnitude > 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                targetRotation, 
                turnSpeed * Time.deltaTime);
        }

        // 3. Handle Chargeable Jump Input
        if (isGrounded)
        {
            // Start charging when space is first pressed down
            if (Input.GetButtonDown("Jump"))
            {
                isChargingJump = true;
                jumpHoldTime = 0f;
            }

            // While holding space, accumulate time
            if (isChargingJump && Input.GetButton("Jump"))
            {
                jumpHoldTime += Time.deltaTime;

                // Auto-jump after reaching the 3-second maximum limit
                if (jumpHoldTime >= maxChargeTime)
                {
                    jumpHoldTime = maxChargeTime;
                    TriggerJump();
                }
            }

            // Jump immediately if released before 3 seconds
            if (isChargingJump && Input.GetButtonUp("Jump"))
            {
                TriggerJump();
            }
        }
        else
        {
            // Cancel charging if player leaves the ground unexpectedly
            isChargingJump = false;
        }
    }

    private void TriggerJump()
    {
        // Calculate force proportional to how long space was held (capped at maxChargeTime)
        float chargeRatio = Mathf.Clamp01(jumpHoldTime / maxChargeTime);
        finalJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, chargeRatio);

        jumpRequested = true;
        isChargingJump = false;
        jumpHoldTime = 0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
 
        if (jumpRequested)
        {
            Jump(finalJumpForce);
            jumpRequested = false;
        }
 
        if (doubleJumpRequested)
        {
            Jump(doubleJumpForce);
            doubleJumpRequested = false;
        }
    }
 
    private void Jump(float force)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.VelocityChange);
    }

    private void MovePlayer()
    {
        float currentSpeed = moveSpeed;
        if (isSprinting)
        {
            currentSpeed *= sprintMultiplier;
        }

        Vector3 targetVelocity = moveDirection * currentSpeed * SpeedMultiplier;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    public bool TryDoubleJump()
    {
        // Require the jump boost power-up to be active
        if (powerUps != null && !powerUps.HasJumpBoost) return false;

        // Not allowed on the ground, or if it was already used
        if (isGrounded || hasDoubleJumped) return false;
 
        doubleJumpRequested = true;
        hasDoubleJumped = true;

        // Trigger the visual/audio effects script
        if (visualEffects != null)
        {
            visualEffects.PlayDoubleJumpEffects();
        }
        if (boingSound != null)
            {
                AudioSource.PlayClipAtPoint(boingSound, transform.position);
            }

        return true;
    }
}