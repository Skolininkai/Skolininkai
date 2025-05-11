using UnityEngine;
using System.Collections.Generic;
using System;

public class PhysicsStressTest : MonoBehaviour
{
    [Header("Stress Test Settings")]
    public int spawnCount = 1000;
    public float spawnRadius = 10f;
    public float explosionForce = 10f;

    [Header("Metrics Settings")]
    [Tooltip("How often to log metrics in seconds")]
    public float metricsUpdateInterval = 1f;

    [Header("Debug Controls")]
    public KeyCode spawnKey = KeyCode.S;
    public KeyCode cleanupKey = KeyCode.C;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private float fpsAccumulator = 0;
    private float fpsNextUpdate = 0;
    private int framesCounted = 0;
    private bool stressTestActive = false;

    void Update()
    {
        HandleInput();
        UpdateMetrics();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(spawnKey)) SpawnObjects();
        if (Input.GetKeyDown(cleanupKey)) CleanupObjects();
    }

    void UpdateMetrics()
    {
        // Update FPS calculations
        fpsAccumulator += Time.timeScale / Time.deltaTime;
        framesCounted++;

        // Log metrics at regular intervals when test is active
        if (Time.realtimeSinceStartup > fpsNextUpdate && stressTestActive)
        {
            // Calculate averages
            float currentFPS = fpsAccumulator / framesCounted;
            long memoryUsage = GC.GetTotalMemory(false) / 1024 / 1024; // MB

            Debug.Log($"Stress Test Metrics:\n" +
                     $"FPS: {currentFPS:0.0}\n" +
                     $"Active Objects: {spawnedObjects.Count}\n" +
                     $"Memory Usage: {memoryUsage} MB");

            // Reset counters
            fpsNextUpdate = Time.realtimeSinceStartup + metricsUpdateInterval;
            fpsAccumulator = 0f;
            framesCounted = 0;
        }
    }

    public void SpawnObjects()
    {
        CleanupObjects();
        stressTestActive = true;
        
        long memoryBefore = GC.GetTotalMemory(false) / 1024 / 1024;
        float startTime = Time.realtimeSinceStartup;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = transform.position + UnityEngine.Random.insideUnitSphere * spawnRadius;
            
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            obj.AddComponent<BoxCollider>();
            
            if (explosionForce > 0)
                rb.AddForce(UnityEngine.Random.insideUnitSphere * explosionForce, ForceMode.Impulse);

            spawnedObjects.Add(obj);
        }

        long memoryAfter = GC.GetTotalMemory(false) / 1024 / 1024;
        float spawnDuration = Time.realtimeSinceStartup - startTime;

        Debug.Log($"=== SPAWN COMPLETE ===\n" +
                 $"Objects Spawned: {spawnCount}\n" +
                 $"Spawn Duration: {spawnDuration:0.00}s\n" +
                 $"Memory Before: {memoryBefore} MB\n" +
                 $"Memory After: {memoryAfter} MB\n" +
                 $"Memory Delta: {memoryAfter - memoryBefore} MB");
    }

    public void CleanupObjects()
    {
        stressTestActive = false;
        long memoryBefore = GC.GetTotalMemory(false) / 1024 / 1024;

        foreach (GameObject obj in spawnedObjects)
            if (obj != null) Destroy(obj);

        spawnedObjects.Clear();
        System.GC.Collect(); // Force garbage collection

        long memoryAfter = GC.GetTotalMemory(false) / 1024 / 1024;

        Debug.Log($"=== CLEANUP COMPLETE ===\n" +
                 $"Memory Before Cleanup: {memoryBefore} MB\n" +
                 $"Memory After Cleanup: {memoryAfter} MB\n" +
                 $"Memory Recovered: {memoryBefore - memoryAfter} MB");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}