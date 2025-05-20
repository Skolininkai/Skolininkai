using UnityEngine;

public class FloorButton : MonoBehaviour
{
    public int buttonIndex = 0;
    private Level2Manager manager;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip longSoundClip;

    // Интервал проигрывания звука (секунды)
    public float soundStartTime = 0f;
    public float soundEndTime = 3f;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !manager.IsShowingPattern() && !manager.IsWaitingForStartButton() && manager.IsWaitingForInput())
        {
            Debug.Log("Player stepped on floor button " + buttonIndex);
            manager.PlayerPress(buttonIndex);

            PlaySoundSegment(soundStartTime, soundEndTime);
        }
    }

    void Update()
    {
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
}
