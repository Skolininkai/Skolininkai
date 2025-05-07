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
    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI scoreText;

    [Header("Lights")]
    public Light roomLight;
    public Light triggerLight;
    public float minFlickerSpeed = 0.05f;
    public float maxFlickerSpeed = 0.2f;
    private Coroutine flickerCoroutine;
    private bool initialLightState;
    private float initialIntensity;

    [Header("Other")]
    public bool isMiniGameActive;
    public float currentTime;
    public int poppedSpheres = 0;
    public int activeSpheres;
    private float fixedCheckRadius;
    private bool gameFinished = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        fixedCheckRadius = (maxSphereSize / 2f) + 0.1f;
        triggerLight.color = new Color32(207, 176, 124, 255);
        roomLight.color = new Color32(207, 176, 124, 255);
    }

    void Start()
    {
        timerText.text = $"{gameDuration:F1}";
        scoreText.text = $"{poppedSpheres}|{totalSpheresToPop}";
    }

    public void StartMiniGame()
    {
        if (isMiniGameActive || gameFinished) return;

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
        initialLightState = roomLight.enabled;
        flickerCoroutine = StartCoroutine(FlickerLight());
        roomLight.color = new Color32(207, 176, 124, 255);
        triggerLight.color = Color.red;
        initialIntensity = roomLight.intensity;
    }

    void Update()
    {
        if (!isMiniGameActive) 
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
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
        }
 
        if (poppedSpheres >= totalSpheresToPop)
        {
            EndMiniGame(true);
            gameFinished = true;
        }

        if (currentTime <= 0)
        {
            EndMiniGame(false);
            return;
        }

        timerText.text = $"{currentTime:F1}";
        scoreText.text = $"{poppedSpheres}|{totalSpheresToPop}";
    }

    public void SpawnSphere()
    {
        if (activeSpheres >= maxActiveSpheres || spherePrefab == null) return;

        int maxAttempts = 20;
        Vector3 spawnPos = Vector3.zero;
        bool validPosition = false;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
            float randomHeight = Random.Range(cylinderMinY, cylinderMaxY);
            Vector3 attemptPos = centerPoint.position + new Vector3(randomCircle.x, randomHeight, randomCircle.y);

            // Use fixed radius for all checks
            if (!Physics.CheckSphere(attemptPos, fixedCheckRadius))
            {
                spawnPos = attemptPos;
                validPosition = true;
                break;
            }
        }
        if (!validPosition) { return; }

        GameObject sphere = Instantiate(spherePrefab, spawnPos, Quaternion.identity);
        float randomSize = Random.Range(minSphereSize, maxSphereSize);
        sphere.transform.localScale = Vector3.one * randomSize;

        Renderer rend = sphere.GetComponent<Renderer>();
        if (rend != null)
        {
            Color emissionColor = Color.red * Mathf.LinearToGammaSpace(0.1f); // Intensity
            rend.material.SetColor("_EmissionColor", emissionColor);
            rend.material.EnableKeyword("_EMISSION");
        }

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
        triggerLight.color = new Color32(207, 176, 124, 255);
        roomLight.color = new Color32(207, 176, 124, 255);
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            roomLight.enabled = initialLightState;
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

    private IEnumerator FlickerLight()
    {
        while (isMiniGameActive)
        {
            roomLight.enabled = !roomLight.enabled;
            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        }
        
        roomLight.enabled = initialLightState;
        roomLight.intensity = Random.Range(initialIntensity * 0.2f, initialIntensity);
    }
}
