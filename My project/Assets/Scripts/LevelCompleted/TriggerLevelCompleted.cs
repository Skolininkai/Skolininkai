using UnityEngine;

public class TriggerLevelCompleted : MonoBehaviour
{
    LevelCompletedMenu menu;
    Timer timer;

    void Start()
    {
        // Find the Level2Manager script in the scene
        menu = FindObjectOfType<LevelCompletedMenu>();

        timer = FindObjectOfType<Timer>();
    }

    // Trigger this method when the player enters the ending trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered level completed Pause menu should open");
            float completionTime = timer != null ? timer.totalTime : 0f;

            menu.OnLevelCompleted(completionTime);
        }
    }
}
