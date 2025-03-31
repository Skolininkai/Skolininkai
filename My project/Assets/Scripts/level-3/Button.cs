using UnityEngine;

public class Button : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger entered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("trying to start minigame");
            AimGameManager.Instance.StartMiniGame();
        }
    }
}
