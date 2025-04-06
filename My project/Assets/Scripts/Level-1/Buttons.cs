using UnityEngine;

public class Buttons : MonoBehaviour
{
    [Header("Pillar Settings")]
    public GameObject[] pillars;
    public float moveSpeed = 5f;
    
    [Header("Button Settings")]
    public GameObject[] sphereButtons;
    public float buttonPressDistance = 5f; // Max distance to press buttons
    public Material normalMaterial;
    public Material highlightedMaterial;
    public Material pressedMaterial;
    public float pressEffectDuration = 0.3f; // Brief visual feedback
    
    [Header("Crosshair Settings")]
    public Texture2D crosshairTexture;
    public Vector2 crosshairSize = new Vector2(32, 32);
    
    // Movement configurations
    private Vector3[] button1Movements = {
        new Vector3(0, 1f, 0),    
        new Vector3(0, -1f, 0),  
        new Vector3(0, 1.5f, 0)     
    };
    
    private Vector3[] button2Movements = {
        new Vector3(0, -1.5f, 0),
        new Vector3(0, 1f, 0),
        new Vector3(0, -1f, 0)
    };
    
    private Vector3[] button3Movements = {
        new Vector3(0, 1f, 0),
        new Vector3(0, 1.5f, 0),
        new Vector3(0, 1f, 0)
    };

    public Vector3[] targetPositions;
    public Vector3[] originalPositions;
    public float[] buttonEffectTimers;
    public GameObject currentlyHighlightedButton;

    public void HandleButtonInteraction(GameObject button)
    {
        for (int i = 0; i < sphereButtons.Length; i++)
        {
            if (button == sphereButtons[i])
            {
                // Highlight button (unless press effect is active)
                if (buttonEffectTimers[i] <= 0)
                {
                    sphereButtons[i].GetComponent<Renderer>().material = highlightedMaterial;
                }
                currentlyHighlightedButton = sphereButtons[i];
                
                // Check for click
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log($"Button{i} pressed.");
                    PressButton(i);
                }
                
                break;
            }
        }
    }

    void Start()
    {
        // Initialize arrays
        originalPositions = new Vector3[pillars.Length];
        targetPositions = new Vector3[pillars.Length];
        buttonEffectTimers = new float[sphereButtons.Length];
        
        // Store original positions
        for (int i = 0; i < pillars.Length; i++)
        {
            originalPositions[i] = pillars[i].transform.position;
            targetPositions[i] = originalPositions[i];
        }
        
        // Set all buttons to normal material
        foreach (GameObject button in sphereButtons)
        {
            button.GetComponent<Renderer>().material = normalMaterial;
        }
        
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Move pillars smoothly
        UpdatePillarPositions();
        
        // Handle button visual effects
        UpdateButtonEffects();
        
        // Handle button interaction
        UpdateButtonInteraction();
    }
    
    public void UpdatePillarPositions()
    {
        for (int i = 0; i < pillars.Length; i++)
        {
            pillars[i].transform.position = Vector3.Lerp(
                pillars[i].transform.position,
                targetPositions[i],
                moveSpeed * Time.deltaTime
            );
        }
    }
    
    public void UpdateButtonEffects()
    {
        // Update visual effect timers
        for (int i = 0; i < buttonEffectTimers.Length; i++)
        {
            if (buttonEffectTimers[i] > 0)
            {
                buttonEffectTimers[i] -= Time.deltaTime;
                if (buttonEffectTimers[i] <= 0)
                {
                    // Only revert if not currently highlighted
                    if (sphereButtons[i] != currentlyHighlightedButton)
                    {
                        sphereButtons[i].GetComponent<Renderer>().material = normalMaterial;
                    }
                }
            }
        }
    }
    
    public void UpdateButtonInteraction()
    {
        // Check for button under crosshair
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        // Reset previous highlight if needed
        if (currentlyHighlightedButton != null && 
            !currentlyHighlightedButton.GetComponent<Renderer>().sharedMaterial.Equals(pressedMaterial))
        {
            // Only revert if press effect isn't active
            int buttonIndex = System.Array.IndexOf(sphereButtons, currentlyHighlightedButton);
            if (buttonIndex >= 0 && buttonEffectTimers[buttonIndex] <= 0)
            {
                currentlyHighlightedButton.GetComponent<Renderer>().material = normalMaterial;
            }
            currentlyHighlightedButton = null;
        }
        
        // Find button under crosshair
        if (Physics.Raycast(ray, out hit, buttonPressDistance))
        {
            HandleButtonInteraction(hit.collider.gameObject);
        }
    }
    
    public void PressButton(int buttonIndex)
    {
        // Set visual effect timer
        buttonEffectTimers[buttonIndex] = pressEffectDuration;
        
        // Visual feedback
        sphereButtons[buttonIndex].GetComponent<Renderer>().material = pressedMaterial;
        
        // Move pillars based on button pressed
        switch (buttonIndex)
        {
            case 0:
                ApplyMovements(button1Movements);
                break;
            case 1:
                ApplyMovements(button2Movements);
                break;
            case 2:
                ApplyMovements(button3Movements);
                break;
        }
    }
    
    public void ApplyMovements(Vector3[] movements)
    {
        for (int i = 0; i < pillars.Length; i++)
        {
            targetPositions[i] += movements[i];
        }
    }
    
    void OnGUI()
    {
        // Draw crosshair
        if (crosshairTexture != null)
        {
            float xMin = (Screen.width - crosshairSize.x) / 2;
            float yMin = (Screen.height - crosshairSize.y) / 2;
            GUI.DrawTexture(new Rect(xMin, yMin, crosshairSize.x, crosshairSize.y), crosshairTexture);
        }
    }
    
    public void ResetPillars()
    {
        for (int i = 0; i < pillars.Length; i++)
        {
            targetPositions[i] = originalPositions[i];
        }
    }
}