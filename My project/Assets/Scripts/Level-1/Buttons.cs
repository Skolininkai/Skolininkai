using UnityEngine;

public class Buttons : MonoBehaviour
{
    // Movement configurations
    private Vector3[] button1Movements = {
        new Vector3(0, 1f, 0),    
        new Vector3(0, -1f, 0),  
        new Vector3(0, 0.5f, 0)     
    };
    
    private Vector3[] button2Movements = {
        new Vector3(0, -0.5f, 0),
        new Vector3(0, 1f, 0),
        new Vector3(0, -1f, 0)
    };
    
    private Vector3[] button3Movements = {
        new Vector3(0, 1f, 0),
        new Vector3(0, 0.5f, 0),
        new Vector3(0, 1f, 0)
    };

    public Vector3[] targetPositions;
    public Vector3[] originalPositions;
    public float[] buttonEffectTimers;
    public GameObject currentlyHighlightedButton;
    
    [Header("Pillar Settings")]
    public GameObject[] pillars;
    public float moveSpeed;
    public float maxHeight;
    public float minHeight;
    
    [Header("Button Settings")]
    public GameObject[] sphereButtons;
    public float buttonPressDistance = 5f;
    public Material normalMaterial;
    public Material highlightedMaterial;
    public Material pressedMaterial;
    public float pressEffectDuration = 0.3f;

    public void HandleButtonInteraction(GameObject button)
    {
        for (int i = 0; i < sphereButtons.Length; i++)
        {
            if (button == sphereButtons[i])
            {
                if (buttonEffectTimers[i] <= 0)
                {
                    sphereButtons[i].GetComponent<Renderer>().material = highlightedMaterial;
                }
                currentlyHighlightedButton = sphereButtons[i];
                
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
        originalPositions = new Vector3[pillars.Length];
        targetPositions = new Vector3[pillars.Length];
        buttonEffectTimers = new float[sphereButtons.Length];
        
        for (int i = 0; i < pillars.Length; i++)
        {
            originalPositions[i] = pillars[i].transform.position;
            targetPositions[i] = originalPositions[i];
        }
        
        foreach (GameObject button in sphereButtons)
        {
            button.GetComponent<Renderer>().material = normalMaterial;
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        UpdatePillarPositions();
        UpdateButtonEffects();
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
        for (int i = 0; i < buttonEffectTimers.Length; i++)
        {
            if (buttonEffectTimers[i] > 0)
            {
                buttonEffectTimers[i] -= Time.deltaTime;
                if (buttonEffectTimers[i] <= 0)
                {
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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        if (currentlyHighlightedButton != null && 
            !currentlyHighlightedButton.GetComponent<Renderer>().sharedMaterial.Equals(pressedMaterial))
        {
            int buttonIndex = System.Array.IndexOf(sphereButtons, currentlyHighlightedButton);
            if (buttonIndex >= 0 && buttonEffectTimers[buttonIndex] <= 0)
            {
                currentlyHighlightedButton.GetComponent<Renderer>().material = normalMaterial;
            }
            currentlyHighlightedButton = null;
        }
        
        if (Physics.Raycast(ray, out hit, buttonPressDistance))
        {
            HandleButtonInteraction(hit.collider.gameObject);
        }
    }
    
    public void PressButton(int buttonIndex)
    {
        buttonEffectTimers[buttonIndex] = pressEffectDuration;
        sphereButtons[buttonIndex].GetComponent<Renderer>().material = pressedMaterial;
        
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
            case 3: // New fourth button case
                ResetPillars();
                break;
        }
    }
    
    public void ApplyMovements(Vector3[] movements)
    {
        for (int i = 0; i < pillars.Length; i++)
        {
            Vector3 newPosition = targetPositions[i] + movements[i];
            
            if (newPosition.y <= maxHeight && newPosition.y >= minHeight)
            {
                targetPositions[i] = newPosition;
            }
            else
            {
                Debug.Log($"Pillar {i} movement blocked - would exceed boundaries");
            }
        }
    }
    
    public void ResetPillars()
    {
        for (int i = 0; i < pillars.Length; i++)
        {
            targetPositions[i] = originalPositions[i];
        }
        Debug.Log("All pillars reset to original positions");
    }
}