using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

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

    public bool isMiniGameActive;
    public float currentTime;
    public int poppedSpheres;
    public int activeSpheres;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartMiniGame()
    {
        if (isMiniGameActive) return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
        player.transform.position = centerPoint.position;
        Debug.Log("starting");

        isMiniGameActive = true;
        currentTime = gameDuration;
        poppedSpheres = 0;
        activeSpheres = 0;

        for (int i = 0; i < maxActiveSpheres; i++)
        {
            SpawnSphere();
        }
    }

    void Update()
    {
        if (!isMiniGameActive) return;

        currentTime -= Time.deltaTime;
        //timerText.text = $"Time: {currentTime:F1}";
        //scoreText.text = $"Popped: {poppedSpheres}/{totalSpheresToPop}";

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.CompareTag("Sphere"))
            {
                Destroy(hit.collider.gameObject);
                activeSpheres = Mathf.Max(0, activeSpheres - 1);
                poppedSpheres++;

                if (poppedSpheres < totalSpheresToPop && activeSpheres < maxActiveSpheres)
                {
                    SpawnSphere();
                }
            }
        }    

        if (poppedSpheres >= totalSpheresToPop)
        {
            EndMiniGame(true);
        }

        if (currentTime <= 0)
        {
            EndMiniGame(false);
            return;
        }
    }

    public void SpawnSphere()
    {
        if (activeSpheres >= maxActiveSpheres || spherePrefab == null) return;

        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        float randomHeight = Random.Range(cylinderMinY, cylinderMaxY);
        
        Vector3 spawnPos = centerPoint.position + new Vector3(randomCircle.x, randomHeight, randomCircle.y);
        
        GameObject sphere = Instantiate(spherePrefab, spawnPos, Quaternion.identity);
        sphere.transform.localScale = Vector3.one * Random.Range(minSphereSize, maxSphereSize);

        Collider col = sphere.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
            StartCoroutine(EnableColliderAfterDelay(col, 0.1f));
        }

        activeSpheres++;
    }

    public void EndMiniGame(bool success)
    {
        isMiniGameActive = false;
        foreach (GameObject sphere in GameObject.FindGameObjectsWithTag("Sphere"))
        {
            Destroy(sphere);
        }
        Debug.Log(success ? "You won!" : "Try again!");
        if (success)
        {
            GameObject doorObj = GameObject.FindWithTag("Door");
            if (doorObj != null)
            {
                doorObj.GetComponent<SlidingDoor>().OpenDoor();
            }
        }
    }
    private IEnumerator EnableColliderAfterDelay(Collider col, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (col != null)
        {
            col.enabled = true;
        }
    }
}
