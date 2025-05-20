using UnityEngine;
using System.Collections;

public class SlidingDoor : MonoBehaviour
{
    public Vector3 openPosition; // Set this in Inspector
    public float openSpeed = 2f;

    private Vector3 closedPosition;
    private bool isOpening = false;
    private Transform doorTransform;

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip doorOpenSound;

    [Header("Sound Interval Settings")]
    public float soundStartTime = 0f;  // Время запуска звука (от начала звука)
    public float soundDuration = 5f;   // Длительность звука, сколько проигрывать (секунд)

    void Start()
    {
        doorTransform = transform;
        closedPosition = doorTransform.position;

        if (audioSource != null)
        {
            audioSource.clip = doorOpenSound;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    public void OpenDoor()
    {
        if (!isOpening)
        {
            StartCoroutine(SlideDoor(openPosition));
        }
    }

    public IEnumerator SlideDoor(Vector3 targetPosition)
    {
        isOpening = true;

        if (audioSource != null && doorOpenSound != null)
        {
            // Запускаем звук с нужного места
            audioSource.time = soundStartTime;
            audioSource.Play();

            // Останавливаем звук через soundDuration секунд
            StartCoroutine(StopSoundAfterDelay(soundDuration));
        }

        Vector3 startPos = doorTransform.position;
        float distance = Vector3.Distance(startPos, targetPosition);
        float duration = distance / openSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            doorTransform.position = Vector3.Lerp(closedPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doorTransform.position = targetPosition;
    }

    private IEnumerator StopSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
