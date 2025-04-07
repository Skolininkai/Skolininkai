using UnityEngine;

public class Button : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("trigger entered");
            
            if (AimGameManager.Instance != null)
            {
                Debug.Log("trying to start minigame");
                AimGameManager.Instance.StartMiniGame();
            }
        }
    }
}
