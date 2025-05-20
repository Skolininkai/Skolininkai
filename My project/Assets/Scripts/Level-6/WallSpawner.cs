using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [System.Serializable]
    public class WallConfig
    {
        public GameObject prefab;
        public Vector3 spawnOffset;
        public Vector3 spawnRotation;
        // public AudioClip moveSound; // Можно добавить
    }

    [Header("Settings")]
    public List<WallConfig> wallConfigs = new List<WallConfig>();
    public Transform startPoint;
    public Transform endPoint;
    public float moveSpeed = 5f;
    public float spawnInterval = 2f;

    [Header("Sound Settings")]
    public Transform playerTransform; // Укажи здесь игрока
    public AudioSource playerAudioSource; // Общий AudioSource на игроке, с цикличным звуком движения стены
    public float maxVolume = 1f;
    public float maxHearingDistance = 15f; // Максимальное расстояние, на котором слышен звук

    private bool isSpawning = true;

    void Start()
    {
        if (playerAudioSource != null)
        {
            playerAudioSource.loop = true;
            playerAudioSource.volume = 0f;
            playerAudioSource.Play();
        }
        StartCoroutine(SpawnWallsRoutine());
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
        GameObject doorObj = GameObject.FindWithTag("Door");
        if (doorObj != null)
        {
            doorObj.GetComponent<SlidingDoor>().OpenDoor();
        }
        if (playerAudioSource != null)
        {
            playerAudioSource.Stop();
        }
    }

    IEnumerator SpawnWallsRoutine()
    {
        while (isSpawning)
        {
            WallConfig config = wallConfigs[Random.Range(0, wallConfigs.Count)];
            Vector3 spawnPosition = startPoint.position +
                                    startPoint.rotation * config.spawnOffset;
            Quaternion rotation = Quaternion.Euler(config.spawnRotation);

            GameObject newWall = Instantiate(config.prefab, spawnPosition, rotation);

            Vector3 endPosition = endPoint.position + startPoint.rotation * config.spawnOffset;

            StartCoroutine(MoveWallRoutine(newWall.transform, endPosition));

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator MoveWallRoutine(Transform wall, Vector3 endPos)
    {
        while (Vector3.Distance(wall.position, endPos) > 0.1f && isSpawning)
        {
            wall.position = Vector3.MoveTowards(
                wall.position,
                endPos,
                moveSpeed * Time.deltaTime
            );

            UpdateSoundVolume(wall.position);

            yield return null;
        }

        Destroy(wall.gameObject);

        // После удаления стены обновим громкость (чтобы звук уменьшился, если больше нет стен рядом)
        UpdateSoundVolume(null);
    }

    void UpdateSoundVolume(Vector3? wallPosition)
    {
        if (playerAudioSource == null || playerTransform == null) return;

        float targetVolume = 0f;

        if (wallPosition.HasValue)
        {
            float dist = Vector3.Distance(playerTransform.position, wallPosition.Value);
            if (dist <= maxHearingDistance)
            {
                // Чем ближе стена — тем громче (линейно)
                targetVolume = maxVolume * (1f - dist / maxHearingDistance);
            }
        }

        // Плавно меняем громкость
        playerAudioSource.volume = Mathf.Lerp(playerAudioSource.volume, targetVolume, Time.deltaTime * 3f);
    }
}
