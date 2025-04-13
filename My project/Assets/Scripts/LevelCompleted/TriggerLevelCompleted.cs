using UnityEngine;

public class TriggerLevelCompleted : MonoBehaviour
{
    LevelCompletedMenu menu;

    void Start()
    {
        // Find the Level2Manager script in the scene
        menu = FindObjectOfType<LevelCompletedMenu>();
    }

    // Trigger this method when the player enters the ending trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered level completed Pause menu should open");
            menu.ShowMenu();
        }
    }
}
