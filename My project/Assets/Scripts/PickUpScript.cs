using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    public Transform holdPos;
    public OutlineSelection outlineSelection;
    public GameObject TruePlayer;

    [Header("Settings")]
    public float throwForce = 500f;
    public float pickUpRange = 5f;
    private float rotationSensitivity = 1f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickUpClip;
    [SerializeField] private AudioClip dropOrThrowClip;

    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private PlayerLook lookScript;
    private bool canDrop = true;
    private int holdLayer;

    void Start()
    {
        holdLayer = LayerMask.NameToLayer("holdLayer");
        lookScript = TruePlayer.GetComponent<PlayerLook>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null)
                TryPickUp();
            else if (canDrop)
            {
                StopClipping();
                DropObject();
            }
        }

        if (heldObj != null)
        {
            MoveObject();
            RotateObject();

            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop)
            {
                StopClipping();
                ThrowObject();
            }
        }
    }

    void TryPickUp()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, pickUpRange))
        {
            if (hit.transform.CompareTag("canPickUp"))
            {
                PickUpObject(hit.transform.gameObject);
                outlineSelection?.ClearHighlight();
            }
        }
    }

    void PickUpObject(GameObject obj)
    {
        if (!obj.TryGetComponent(out Rigidbody rb)) return;

        heldObj = obj;
        heldObjRb = rb;
        heldObjRb.isKinematic = true;
        heldObj.transform.parent = holdPos;
        heldObj.layer = holdLayer;
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        if (audioSource != null && pickUpClip != null)
        {
            audioSource.PlayOneShot(pickUpClip);
        }
    }

    void DropObject()
    {
        ResetObjectPhysics();
        heldObj = null;
        if (audioSource != null && dropOrThrowClip != null)
        {
            audioSource.PlayOneShot(dropOrThrowClip);
        }
    }

    void ThrowObject()
    {
        ResetObjectPhysics();
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
        if (audioSource != null && dropOrThrowClip != null)
        {
            audioSource.PlayOneShot(dropOrThrowClip);
        }
    }

    void ResetObjectPhysics()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 3;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
    }

    void MoveObject() => heldObj.transform.position = holdPos.position;

    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))
        {
            canDrop = false;
            if (lookScript != null)
            {
                lookScript.sensX = 0f;
                lookScript.sensY = 0f;
            }

            float xRot = Input.GetAxis("Mouse X") * rotationSensitivity;
            float yRot = Input.GetAxis("Mouse Y") * rotationSensitivity;

            heldObj.transform.Rotate(Vector3.down, xRot);
            heldObj.transform.Rotate(Vector3.right, yRot);
        }
        else
        {
            if (lookScript != null)
            {
                lookScript.sensX = 100f;
                lookScript.sensY = 100f;
            }
            canDrop = true;
        }
    }

    void StopClipping()
    {
        float clipRange = Vector3.Distance(heldObj.transform.position, transform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, clipRange);

        if (hits.Length > 1)
        {
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
        }
    }
}
