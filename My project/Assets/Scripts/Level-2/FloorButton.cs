using UnityEngine;

public class FloorButton : MonoBehaviour
{
    // Set the index for this button in the Inspector (0 for the first button, 1 for the second, etc.)
    public int buttonIndex = 0;
    private Level2Manager manager;

    void Start()
    {
        // Find the Level2Manager script in the scene
        manager = FindObjectOfType<Level2Manager>();
    }

    // Trigger this method when the player steps on the floor button
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !manager.IsShowingPattern() && !manager.IsWaitingForStartButton() && manager.IsWaitingForInput())
        {
            Debug.Log("Player stepped on floor button " + buttonIndex);
            manager.PlayerPress(buttonIndex);
        }
    }
}
