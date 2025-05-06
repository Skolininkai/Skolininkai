using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [Header("Settings")]
    public List<GameObject> wallPrefabs; // Prefabs with colliders/meshes
    public Transform startPoint;         // Where walls spawn
    public Transform endPoint;           // Where walls get destroyed
    public float moveSpeed = 5f;         // Wall movement speed
    public float spawnInterval = 2f;     // Time between spawns

    private bool isSpawning = true;

    void Start()
    {
        StartCoroutine(SpawnWallsRoutine());
    }

    // Call this via UnityEvent or another script when player enters trigger
    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    IEnumerator SpawnWallsRoutine()
    {
        while (isSpawning)
        {
            // Spawn random wall from list
            GameObject wallPrefab = wallPrefabs[Random.Range(0, wallPrefabs.Count)];
            GameObject newWall = Instantiate(wallPrefab, startPoint.position, startPoint.rotation);
            
            // Start moving the wall
            StartCoroutine(MoveWallRoutine(newWall.transform));
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator MoveWallRoutine(Transform wall)
    {
        while (Vector3.Distance(wall.position, endPoint.position) > 0.1f && isSpawning)
        {
            wall.position = Vector3.MoveTowards(
                wall.position,
                endPoint.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        
        Destroy(wall.gameObject);
    }
}