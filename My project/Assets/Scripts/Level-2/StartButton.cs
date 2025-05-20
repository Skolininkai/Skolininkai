using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    private bool isPlayerNear = false;
    private Level2Manager manager;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip longSoundClip;

    // Отрезок звука для воспроизведения (в секундах)
    public float soundStartTime = 0f;
    public float soundEndTime = 5f;

    private bool isPlayingSegment = false;

    void Start()
    {
        manager = FindObjectOfType<Level2Manager>();

        if (audioSource != null && longSoundClip != null)
        {
            audioSource.clip = longSoundClip;
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (manager.IsWaitingForStartButton())
            {
                Debug.Log("E pressed, starting or continuing the game.");
                manager.StartStep();

                PlaySoundSegment(soundStartTime, soundEndTime);
            }
        }

        // Контроль окончания воспроизведения отрезка звука
        if (isPlayingSegment && audioSource.isPlaying)
        {
            if (audioSource.time >= soundEndTime)
            {
                audioSource.Stop();
                isPlayingSegment = false;
            }
        }
    }

    void PlaySoundSegment(float startTime, float endTime)
    {
        if (audioSource == null || longSoundClip == null)
            return;

        if (startTime < 0f || endTime > longSoundClip.length || startTime >= endTime)
        {
            Debug.LogWarning("Неверные значения времени воспроизведения звука.");
            return;
        }

        audioSource.time = startTime;
        audioSource.Play();
        isPlayingSegment = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (manager.IsWaitingForStartButton())
            {
                Hints.instance.ShowHint("Press 'E' to start the sequence", 2);
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
