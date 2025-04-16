using UnityEngine;
using System.Collections;

public class Level2Manager : MonoBehaviour
{
    public GameObject[] screenPanels; // Assign screen panels (Green, Red, Blue, Yellow)
    public GameObject[] floorButtons; // Assign ground buttons in the Inspector
    public Light[] stepLights; // Assign the overhead lights in the Inspector
    public Material greenMaterial; // Green material for completed steps
    public Material defaultMaterial; // Default material for overhead lights

    private int step = 1;
    private int currentIndex = 0;
    private int[] sequence;
    private bool playerTurn = false;
    private bool waitingForInput = false;
    private bool showingPattern = false;
    private bool waitingForStartButton = true;
    private bool[] buttonCooldowns;

    public virtual void Start()
    {
        GeneratePattern();
        buttonCooldowns = new bool[screenPanels.Length]; // Initialize cooldowns
    }

    public virtual bool IsWaitingForInput()
    {
        return waitingForInput;
    }
    public virtual bool IsWaitingForStartButton()
    {
        return waitingForStartButton;
    }
    public virtual bool IsShowingPattern()
    {
        return showingPattern;
    }

    public void GeneratePattern()
    {
        sequence = new int[5]; // Set for 5 steps
        for (int i = 0; i < sequence.Length; i++)
        {
            sequence[i] = Random.Range(0, screenPanels.Length); // Randomly assign colors
        }
        Debug.Log("New pattern generated.");
    }

    public void StartStep()
    {
        if (waitingForStartButton)
        {
            waitingForStartButton = false;
            StartCoroutine(ShowPattern(step));
        }
    }

    IEnumerator ShowPattern(int currentStep)
    {
        waitingForInput = false;
        showingPattern = true;
        playerTurn = false;
        currentIndex = 0;

        for (int i = 0; i < currentStep; i++)
        {
            int colorIndex = sequence[i];
            yield return new WaitForSeconds(0.5f);
            screenPanels[colorIndex].SetActive(false);
            yield return new WaitForSeconds(1f);
            screenPanels[colorIndex].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        playerTurn = true;
        showingPattern = false;
        waitingForInput = true;

    }

    public virtual void PlayerPress(int buttonIndex)
    {
        if (playerTurn && waitingForInput && !buttonCooldowns[buttonIndex])
        {
            if (buttonIndex == sequence[currentIndex])
            {
                buttonCooldowns[buttonIndex] = true; // Activate cooldown for this button
                StartCoroutine(EnableButtonAfterDelay(buttonIndex, 1f)); // Start cooldown timer
                currentIndex++;
                if (currentIndex == step)
                {
                    StepCompleted();
                }
            }
            else
            {
                ResetGame();
            }
        }
    }

    IEnumerator EnableButtonAfterDelay(int buttonIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        buttonCooldowns[buttonIndex] = false;
    }

    void StepCompleted()
    {
        stepLights[step - 1].color = Color.green; // Change light to green
        step++;

        if (step > sequence.Length)
        {
            Debug.Log("Level Completed!");
            waitingForInput = false;

            GameObject doorObj = GameObject.FindWithTag("Door");
            if (doorObj != null)
            {
                doorObj.GetComponent<SlidingDoor>().OpenDoor();
            }
        }
        else
        {
            Debug.Log("Step completed. Press the button to start the next step.");
            waitingForStartButton = true; // Player must press the button to continue
        }
    }

    void ResetGame()
    {
        Debug.Log("Wrong sequence! Restarting");
        for (int i = 0; i < stepLights.Length; i++)
        {
            stepLights[i].color = Color.red;
        }
        step = 1;
        GeneratePattern();
        waitingForStartButton = true; // Player must press the button to restart
        waitingForInput = false;
    }
}
