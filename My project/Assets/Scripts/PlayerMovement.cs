using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;
    [SerializeField] GameObject player;
    [SerializeField] GameObject cameraPosition;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float crouchSpeed = 3f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftShift;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;
    CapsuleCollider playerCollider;

    RaycastHit slopeHit;
    bool isCrouching = false;
    bool isUncrouching = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = player.GetComponent<CapsuleCollider>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        float sphereRadius = 0.5f;
        float sphereCastDistance = (playerCollider.height / 2) + groundDistance; // playerHeight * 0.5f + 0.05f;
        isGrounded = Physics.SphereCast(transform.position, sphereRadius, Vector3.down, out RaycastHit hit, sphereCastDistance, groundMask);

        Debug.DrawRay(transform.position, Vector3.down, Color.yellow, playerHeight * 0.5f + groundDistance);


        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        if (isUncrouching)
        {
            SmoothUncrouch();
        }
        if (isCrouching && !Input.GetKey(crouchKey))
        {
            Uncrouch();
        }

        Debug.DrawRay(transform.position + Vector3.up * (playerCollider.height / 2), Vector3.up, Color.red, playerHeight - (playerHeight / 2));
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

        if (Input.GetKeyDown(crouchKey))
        {
            Crouch();
        }
        if (Input.GetKeyUp(crouchKey))
        {
            Uncrouch();
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Crouch()
    {
        if (!isCrouching)
        {
            playerCollider.height = playerHeight / 2;
            playerCollider.center = new Vector3(0, -0.25f, 0);
            cameraPosition.transform.localPosition = new Vector3(0, 0.3f, 0);
            isCrouching = true;
            isUncrouching = false;
        }
    }
    void Uncrouch()
    {
        if (isCrouching) 
        {
            float raycastDistance = playerHeight - (playerHeight / 2);
            Vector3 raycastOrigin = transform.position + Vector3.up * (playerCollider.height / 2);

            if (!Physics.SphereCast(raycastOrigin, playerCollider.radius, Vector3.up, out RaycastHit hit, 0.5f))
            {
                isUncrouching = true;
            }
            else
            {
                
            }
        }
    }
    void SmoothUncrouch()
    {
        float targetHeight = playerHeight;
        float targetCameraY = 0.65f;

        playerCollider.height = Mathf.Lerp(playerCollider.height, targetHeight, Time.deltaTime * 10f);
        playerCollider.center = Vector3.Lerp(playerCollider.center, Vector3.zero, Time.deltaTime * 10f);

        Vector3 targetCameraPosition = new Vector3(0, targetCameraY, 0);
        cameraPosition.transform.localPosition = Vector3.Lerp(cameraPosition.transform.localPosition, targetCameraPosition, Time.deltaTime * 10f);

        if (Mathf.Abs(playerCollider.height - targetHeight) < 0.01f)
        {
            playerCollider.height = targetHeight;
            playerCollider.center = Vector3.zero;
            cameraPosition.transform.localPosition = targetCameraPosition;
            isCrouching = false;
            isUncrouching = false;
        }
    }
    void ControlSpeed()
    {
        if (isCrouching)
        {
            moveSpeed = crouchSpeed;
        }
        else
        {
           moveSpeed = 6f;
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Force);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Force);
        }
    }
        private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}
