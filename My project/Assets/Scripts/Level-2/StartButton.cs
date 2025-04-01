using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    private bool isPlayerNear = false;
    private Level2Manager manager;

    void Start()
    {
        manager = FindObjectOfType<Level2Manager>();
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (manager.IsWaitingForStartButton())
            {
                Debug.Log("E pressed, starting or continuing the game.");
                manager.StartStep();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (manager.IsWaitingForStartButton())
            {
                Debug.Log("Press 'E' to start or continue.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
