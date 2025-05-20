using System.Security;
using System.Text.RegularExpressions;
using NUnit.Framework.Internal;
using UnityEngine;
using System.Collections;



public class Buttons : MonoBehaviour
{
    // Movement configurations
    private Vector3[] button1VerticalMovements = {
        new Vector3(0, 1f, 0),    
        new Vector3(0, -1f, 0),  
        new Vector3(0, 0.5f, 0)     
    };

    private Vector3[] button1HorizontalMovements = {
        new Vector3(0, 0, 1f),    
        new Vector3(0, 0, -0.5f),  
        new Vector3(0, 0, 0.5f)     
    };
    
    private Vector3[] button2VerticalMovements = {
        new Vector3(0, -0.5f, 0),
        new Vector3(0, 1f, 0),
        new Vector3(0, -1f, 0)
    };

    private Vector3[] button2HorizontalMovements = {
        new Vector3(0, 0, -0.5f),    
        new Vector3(0, 0, 0.5f),  
        new Vector3(0, 0, 1f)     
    };
    
    private Vector3[] button3VerticalMovements = {
        new Vector3(0, 1f, 0),
        new Vector3(0, 0.5f, 0),
        new Vector3(0, 1f, 0)
    };

    private Vector3[] button3HorizontalMovements = {
        new Vector3(0, 0, 0.5f),    
        new Vector3(0, 0, 0.5f),  
        new Vector3(0, 0, -0.5f)     
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
    public float maxWidth;
    public float minWidth;
    
    [Header("Button Settings")]
    public GameObject[] sphereButtons;
    public float buttonPressDistance = 5f;
    public Material normalMaterial;
    public Material highlightedMaterial;
    public Material pressedMaterial;
    public float pressEffectDuration = 0.3f;
    public Animator ButtonAnimator1;
    public Animator ButtonAnimator2;
    public Animator ButtonAnimator3;
    public Animator ButtonAnimator4;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip defaultButtonClickClip;   
    [SerializeField] private AudioClip resetButtonClickClip;
    [SerializeField] private AudioSource pillarMoveAudioSource;
    [SerializeField] private AudioClip pillarMoveClip;

    [Header("Sound")]
    public AudioSource platformAudioSource;
    private bool platformIsMoving = false;
    
    

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

                if (Input.GetKeyDown(KeyCode.E))
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
            originalPositions[i] = pillars[i].transform.localPosition;
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
        bool anyPillarMoving = false;

        for (int i = 0; i < pillars.Length; i++)
        {
            Vector3 currentPos = pillars[i].transform.localPosition;
            Vector3 targetPos = targetPositions[i];

            float distance = Vector3.Distance(currentPos, targetPos);
            if (distance > 0.01f) // платформа реально движется
            {
                anyPillarMoving = true;
            }

            pillars[i].transform.localPosition = Vector3.Lerp(
                currentPos,
                targetPos,
                moveSpeed * Time.deltaTime
            );
        }

        // Обработка звука движения
        if (anyPillarMoving)
        {
            if (!platformIsMoving)
            {
                platformIsMoving = true;
                platformAudioSource.Play();
            }
        }
        else
        {
            if (platformIsMoving)
            {
                platformIsMoving = false;
                StartCoroutine(FadeOutSound()); // или StartCoroutine(FadeOutSound());
            }
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
    
    private IEnumerator FadeOutSound(float duration = 0.5f)
    {
        float startVolume = platformAudioSource.volume;

        while (platformAudioSource.volume > 0)
        {
            platformAudioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        platformAudioSource.Stop();
        platformAudioSource.volume = startVolume;
    }
    
    public void UpdateButtonInteraction()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, buttonPressDistance))
        {
            HandleButtonInteraction(hit.collider.gameObject);
        }
        else
        {
            if (currentlyHighlightedButton != null)
            {
                int buttonIndex = System.Array.IndexOf(sphereButtons, currentlyHighlightedButton);
                if (buttonIndex >= 0 && buttonEffectTimers[buttonIndex] <= 0 &&
                !currentlyHighlightedButton.GetComponent<Renderer>().material.Equals(pressedMaterial))
                {
                    currentlyHighlightedButton.GetComponent<Renderer>().material = normalMaterial;
                }
                currentlyHighlightedButton = null;
            }
        }
    }

    public void PressButton(int buttonIndex)
    {
        buttonEffectTimers[buttonIndex] = pressEffectDuration;
        sphereButtons[buttonIndex].GetComponent<Renderer>().material = pressedMaterial;

        if (audioSource != null)
        {
            if (buttonIndex == 3 && resetButtonClickClip != null)
            {
                audioSource.PlayOneShot(resetButtonClickClip);
            }
            else if (defaultButtonClickClip != null)
            {
                audioSource.PlayOneShot(defaultButtonClickClip);
            }   
        }

        switch (buttonIndex)
        {
            case 0:
                ButtonAnimator1.SetTrigger("Press");
                ApplyMovements(button1VerticalMovements, button1HorizontalMovements);   
                break;
            case 1:
                ButtonAnimator2.SetTrigger("Press");
                ApplyMovements(button2VerticalMovements, button2HorizontalMovements);
                break;
            case 2:
                ButtonAnimator3.SetTrigger("Press");
                ApplyMovements(button3VerticalMovements, button3HorizontalMovements);
                break;
            case 3:
                ButtonAnimator4.SetTrigger("Press");
                ResetPillars();
                break;
        }
    }

    public string CheckPillarType(GameObject pillar)
    {
        if(pillar.name.Contains("Vertical"))
        {
            return "Vertical";
        }
        else if(pillar.name.Contains("Horizontal"))
        {
            return "Horizontal";
        }

        return "";
    }

    public int CheckPillarNumber(GameObject pillar)
    {
         Match match = Regex.Match(
            pillar.name,
            @"(\d+)$",
            RegexOptions.IgnoreCase
        );

        if (match.Success)
        {
            int pillarNumber = int.Parse(match.Groups[1].Value);
            return pillarNumber;
        }

        return 0;
    }
    
    public void ApplyMovements(Vector3[] verticalMovements, Vector3[] horizontalMovements)
    {
        if (pillarMoveAudioSource != null && pillarMoveClip != null)
        {
            pillarMoveAudioSource.PlayOneShot(pillarMoveClip);
        }
        for (int i = 0; i < pillars.Length; i++)
        {
            string pillarType = CheckPillarType(pillars[i]);
            int pillarNumber = CheckPillarNumber(pillars[i]);

            if(pillarType == "Vertical")
            {
                Vector3 newVerticalPosition;
                newVerticalPosition = targetPositions[i] + verticalMovements[pillarNumber-1];

                if (newVerticalPosition.y <= maxHeight && newVerticalPosition.y >= minHeight)
                {
                    if (Vector3.Distance(targetPositions[i], newVerticalPosition) > 0.01f)
                    {
                        targetPositions[i] = newVerticalPosition;
                    }
                }
                else
                {
                    Debug.Log($"old position: {targetPositions[i]}   new position: {newVerticalPosition}");
                    Debug.Log($"Pillar{pillarType}{pillarNumber} movement blocked - would exceed boundaries");
                }
            }
            else if(pillarType == "Horizontal") 
            {
                Vector3 newHorizontalPosition;
                newHorizontalPosition = targetPositions[i] + horizontalMovements[pillarNumber-1];
                
                if (newHorizontalPosition.z <= maxWidth && newHorizontalPosition.z >= minWidth)
                {
                    if (Vector3.Distance(targetPositions[i], newHorizontalPosition) > 0.01f)
                    {
                        targetPositions[i] = newHorizontalPosition;
                    }
                }
                else
                {
                    Debug.Log($"old position: {targetPositions[i]}   new position: {newHorizontalPosition}");
                    Debug.Log($"Pillar{pillarType}{pillarNumber} movement blocked - would exceed boundaries");
                }
            }
            else 
            {
                Debug.Log("Error in ApplyMovements()");
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