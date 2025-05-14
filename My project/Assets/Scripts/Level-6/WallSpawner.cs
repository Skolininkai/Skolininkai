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
        public Vector3 spawnRotation; // Adjust rotations per-prefab in Inspector
    }

    [Header("Settings")]
    public List<WallConfig> wallConfigs = new List<WallConfig>(); // Replace wallPrefabs with this
    public Transform startPoint;
    public Transform endPoint;
    public float moveSpeed = 5f;
    public float spawnInterval = 2f;

    private bool isSpawning = true;

    void Start()
    {
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
    }

    IEnumerator SpawnWallsRoutine()
    {
        while (isSpawning)
        {
            // Pick a random wall configuration
            WallConfig config = wallConfigs[Random.Range(0, wallConfigs.Count)];
            Vector3 spawnPosition = startPoint.position + 
                                startPoint.rotation * config.spawnOffset;
            //Debug.Log($"Spawning wall at: {spawnPosition}");
            Quaternion rotation = Quaternion.Euler(config.spawnRotation);
            
            // Instantiate with custom rotation
            GameObject newWall = Instantiate(
                config.prefab,
                spawnPosition,
                rotation // Use the prefab's defined rotation
            );

            Vector3 endPosition = endPoint.position + startPoint.rotation * config.spawnOffset;

            //Debug.Log($"Wall actual position: {newWall.transform.position}");
            StartCoroutine(MoveWallRoutine(newWall.transform, endPosition));
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator MoveWallRoutine(Transform wall, Vector3 endPos)
    {
        // (Same as before)
        while (Vector3.Distance(wall.position, endPos) > 0.1f && isSpawning)
        {
            wall.position = Vector3.MoveTowards(
                wall.position,
                endPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        Destroy(wall.gameObject);
    }
}