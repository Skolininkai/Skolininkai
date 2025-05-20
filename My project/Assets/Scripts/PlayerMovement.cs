using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;
    [SerializeField] GameObject player;
    [SerializeField] GameObject cameraPosition;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    private float roMoveSpeed;
    [SerializeField] float crouchSpeed = 3f;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField] float gravityMultiplier = 2.5f;
    float movementMultiplier = 10f;
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float footstepInterval = 0.5f;

    private float footstepTimer;

    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioSource jumpAudioSource;

    [SerializeField] private AudioClip landingClip;
    [SerializeField] private AudioSource landingAudioSource;

    private bool wasGroundedLastFrame;


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

    [Header("Crouch Settings")]
    [SerializeField] float crouchHeight = 1f;
    [SerializeField] float crouchCameraOffsetY = 0.3f;
    [SerializeField] float crouchColliderCenterOffsetY = -0.25f;
    [SerializeField] float defaultCameraY = 0.65f;
    [SerializeField] float uncrouchLerpSpeed = 10f;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;
    CapsuleCollider playerCollider;

    RaycastHit slopeHit;
    bool isCrouching = false;
    bool isUncrouching = false;

    #region Exposed for Testing
    #if UNITY_INCLUDE_TESTS
    public Transform TestOrientation { get => orientation; set => orientation = value; }
    public GameObject TestPlayer { get => player; set => player = value; }
    public GameObject TestCameraPosition { get => cameraPosition; set => cameraPosition = value; }
    public Transform TestGroundCheck { get => groundCheck; set => groundCheck = value; }
    public LayerMask TestGroundMask { get => groundMask; set => groundMask = value; }
    public CapsuleCollider TestPlayerCollider { get => playerCollider; set => playerCollider = value; }
    public float TestplayerHeight => playerHeight;
    public float TestCrouchHeight => crouchHeight;
    public float TestCrouchCameraY => crouchCameraOffsetY;
    public float TestMoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public bool TestIsGrounded => isGrounded;
    public bool TestIsCrouching => isCrouching;
    #endif
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = player.GetComponent<CapsuleCollider>();
        rb.freezeRotation = true;

        playerCollider.height = playerHeight;
        playerCollider.center = Vector3.zero;

        roMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        //float sphereRadius = 0.4f;
        //float sphereCastDistance = (playerCollider.height / 2) + groundDistance; // playerHeight * 0.5f + 0.05f;
        //isGrounded = Physics.SphereCast(transform.position, sphereRadius, Vector3.down, out RaycastHit hit, sphereCastDistance, groundMask);
        Vector3 colliderBottom = transform.position + playerCollider.center - Vector3.up * (playerCollider.height / 2);
        // float skinWidth = 0.1f;
        // Vector3 sphereCastStart = colliderBottom + Vector3.up * skinWidth;
        // float castDistance = groundDistance + skinWidth;
        // isGrounded = Physics.SphereCast(
        //     sphereCastStart, 
        //     sphereRadius, 
        //     Vector3.down, 
        //     out RaycastHit hit, 
        //     castDistance,
        //     groundMask
        // );

        Debug.DrawRay(colliderBottom, Vector3.down, Color.yellow, 0.5f);

        float checkRadius = 0.4f; // Adjust based on player size
        isGrounded = Physics.CheckSphere(colliderBottom, checkRadius, groundMask);


        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        if (isCrouching && !Input.GetKey(crouchKey))
        {
            Uncrouch();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        Vector3 raycastOrigin = transform.position + Vector3.up * (playerCollider.center.y + playerCollider.height / 2);
        float neededHeight = playerHeight - playerCollider.height;
        Debug.DrawRay(raycastOrigin, Vector3.up, Color.red, neededHeight);
        if (Physics.Raycast(raycastOrigin, Vector3.up, out RaycastHit hit2, neededHeight))
        {
            //Debug.Log("raycast hit");
        }
        MyInput();
        ControlDrag();
        ControlSpeed();
        HandleFootsteps();

        if (!wasGroundedLastFrame && isGrounded)
        {
            // Игрок только что приземлился
            if (landingAudioSource != null && landingClip != null)
            {
                landingAudioSource.PlayOneShot(landingClip);
            }
        }

        wasGroundedLastFrame = isGrounded;
    }
    private void FixedUpdate()
    {
        MovePlayer();
        GravityModifier();

        if (isUncrouching && !Input.GetKey(crouchKey))
        {
            SmoothUncrouch();
        }
    }

    private void HandleFootsteps()
    {
        bool isMoving = horizontalMovement != 0 || verticalMovement != 0;

        if (isGrounded && isMoving)
        {
            if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.clip = footstepClip;
                footstepAudioSource.loop = true;
                footstepAudioSource.Play();
            }
        }
        else
        {
            if (footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
        }
    }

    public void Crouch()
    {
        if (!isCrouching)
        {
            playerCollider.height = crouchHeight;
            playerCollider.center = new Vector3(0, crouchColliderCenterOffsetY, 0);
            cameraPosition.transform.localPosition = new Vector3(0, crouchCameraOffsetY, 0);
            isCrouching = true;
            isUncrouching = false;

            // ЗАМЕДЛЯЕМ ЗВУК ШАГОВ
            if (footstepAudioSource != null)
            {
                footstepAudioSource.pitch = 0.6f;
            }
        }
    }

    public void Uncrouch()
    {
        if (isCrouching)
        {
            if (CanUncrouch())
            {
                isUncrouching = true;

                // ВОЗВРАЩАЕМ НОРМАЛЬНУЮ СКОРОСТЬ ЗВУКА
                if (footstepAudioSource != null)
                {
                    footstepAudioSource.pitch = 1.0f;
                }
            }
        }
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

    void GravityModifier() 
    {
        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1), ForceMode.Acceleration);
        }
    }

    public void Jump()
    {
        if (isUncrouching) return;
        if (isCrouching && !CanUncrouch()) return;
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            // Воспроизводим звук прыжка
            if (jumpAudioSource != null && jumpClip != null)
            {
                jumpAudioSource.PlayOneShot(jumpClip);
            }
        }
    }


    public bool CanUncrouch()
    {
        Vector3 raycastOrigin = transform.position + Vector3.up * (playerCollider.center.y + playerCollider.height / 2);
        float neededHeight = playerHeight - playerCollider.height + 0.1f;
        bool cantUncrouch = Physics.Raycast(raycastOrigin, Vector3.up, out RaycastHit hit, neededHeight);
        return !cantUncrouch;
    }

    
    
    void SmoothUncrouch()
    {
        float targetHeight = playerHeight;

        playerCollider.height = Mathf.Lerp(playerCollider.height, targetHeight, Time.deltaTime * uncrouchLerpSpeed);
        playerCollider.center = Vector3.Lerp(playerCollider.center, Vector3.zero, Time.deltaTime * uncrouchLerpSpeed);

        Vector3 targetCameraPosition = new Vector3(0, defaultCameraY, 0);
        cameraPosition.transform.localPosition = Vector3.Lerp(cameraPosition.transform.localPosition, targetCameraPosition, Time.deltaTime * uncrouchLerpSpeed);

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
           moveSpeed = roMoveSpeed;
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
