using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Tooltip("The height at which the player is considered to have fallen off")]
    public float fallHeight = -10f;
    
    [Tooltip("Reference to the player's character controller (if using one)")]
    public CharacterController characterController;
    
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        // Store the initial position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;
        
        // If characterController reference isn't set, try to get it automatically
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        // Check if player has fallen below the threshold
        if (transform.position.y < fallHeight)
        {
            ResetPlayerPosition();
        }
    }

    public void ResetPlayerPosition()
    {
        // Disable character controller if it exists (to prevent collision issues)
        if (characterController != null)
        {
            characterController.enabled = false;
        }
        
        // Reset position and rotation
        transform.position = startPosition;
        transform.rotation = startRotation;
        
        // Re-enable character controller if it exists
        if (characterController != null)
        {
            characterController.enabled = true;
        }
        
        Debug.Log("Player fell off and was reset to start position");
    }
}