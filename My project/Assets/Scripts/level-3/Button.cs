using UnityEngine;

public class Button : MonoBehaviour
{
    public AudioSource audioSource;  // Назначай через инспектор
    public AudioClip activationSound; // Звук активации кнопки

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("trigger entered");

            // Проигрываем звук, если он задан
            if (audioSource != null && activationSound != null)
            {
                audioSource.PlayOneShot(activationSound);
            }

            if (AimGameManager.Instance != null)
            {
                Debug.Log("trying to start minigame");
                AimGameManager.Instance.StartMiniGame();
            }
        }
    }
}
