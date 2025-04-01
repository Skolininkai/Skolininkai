using UnityEngine;
using System.Collections.Generic;
using UnityEngine.ProBuilder.Shapes;

public class AimGameManager : MonoBehaviour
{
    public static AimGameManager Instance;

    [Header("Settings")]
    public Transform centerPoint;
    public GameObject spherePrefab;
    public float spawnRadius = 5f;
    public float gameDuration = 30f;
    public int maxActiveSpheres = 5;
    public int totalSpheresToPop = 50;

    [Header("Cylinder Settings")]
    public float cylinderMinY = -2f; // Lowest Y position
    public float cylinderMaxY = 5f;  // Highest Y position
    public float minSphereSize = 0.3f;
    public float maxSphereSize = 1.5f;

    [Header("UI")]
    //public TMPro.TextMeshProUGUI timerText;
    //public TMPro.TextMeshProUGUI scoreText;

    private bool isMiniGameActive;
    private float currentTime;
    private int poppedSpheres;
    private int activeSpheres;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartMiniGame()
    {
        if (isMiniGameActive) return;

        // Reset player position
        GameObject.FindWithTag("Player").transform.position = centerPoint.position;
        Debug.Log("starting");

        // Reset game state
        isMiniGameActive = true;
        currentTime = gameDuration;
        poppedSpheres = 0;
        activeSpheres = 0;

        // Spawn initial spheres
        for (int i = 0; i < maxActiveSpheres; i++)
        {
            SpawnSphere();
        }
    }

    void Update()
    {
        if (!isMiniGameActive) return;

        // Update timer
        currentTime -= Time.deltaTime;
        //timerText.text = $"Time: {currentTime:F1}";
        //scoreText.text = $"Popped: {poppedSpheres}/{totalSpheresToPop}";

        // Check for failure
        if (currentTime <= 0)
        {
            EndMiniGame(false);
            return;
        }

        // Raycast to pop spheres
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Center of screen
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.CompareTag("Sphere"))
            {
                Destroy(hit.collider.gameObject);
                activeSpheres--;
                poppedSpheres++;

                // Spawn new sphere if needed
                if (poppedSpheres < totalSpheresToPop && activeSpheres < maxActiveSpheres)
                {
                    SpawnSphere();
                }

                // Check for win
                if (poppedSpheres >= totalSpheresToPop)
                {
                    EndMiniGame(true);
                }
            }
        }
    }

    void SpawnSphere()
    {
        // Random angle and height within cylindrical bounds
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        float randomHeight = Random.Range(cylinderMinY, cylinderMaxY); // Add these variables
        
        Vector3 spawnPos = centerPoint.position + new Vector3(randomCircle.x, randomHeight, randomCircle.y);
        
        GameObject sphere = Instantiate(spherePrefab, spawnPos, Quaternion.identity);
        sphere.transform.localScale = Vector3.one * Random.Range(minSphereSize, maxSphereSize);
        activeSpheres++;
    }

    void EndMiniGame(bool success)
    {
        isMiniGameActive = false;
        // Clear remaining spheres
        foreach (GameObject sphere in GameObject.FindGameObjectsWithTag("Sphere"))
        {
            Destroy(sphere);
        }
        // Show result (expand this)
        Debug.Log(success ? "You won!" : "Try again!");
        if (success)
        {
            GameObject.FindWithTag("Door").GetComponent<SlidingDoor>().OpenDoor();
        }
    }
}
