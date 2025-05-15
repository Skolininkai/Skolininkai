using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompletedMenu : MonoBehaviour
{
    private static LevelCompletedMenu instance;
    public GameObject menuUI;

    [Header("Star Rating")]
    public Image[] starImages;
    public Sprite emptyStar;
    public Sprite filledStar;
    public float threeStarTime = 60f;
    public float twoStarTime = 90f;
    public float oneStarTime = 120;
    private int currentRating = 0;

    [Header("Score Tracking")]
    public TMP_Text timeText;

    private float completionTime;

    void Start()
    {
        if (menuUI == null)
        {
            menuUI = GameObject.Find("LevelCompletedMenu");
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (menuUI != null)
            {
                menuUI.SetActive(false);
            }
        }
    }

    public void ShowMenu()
    {
        Debug.Log("ShowMenu is called");
        menuUI.SetActive(true);
        Time.timeScale = 0f;

        // Show and unlock the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideMenu()
    {
        menuUI.SetActive(false);
        Time.timeScale = 1f;

        // Hide and lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private int CalculateRating(float time)
    {
       
        if (time <= threeStarTime) return 3;
        if (time <= twoStarTime) return 2;
        if (time <= oneStarTime) return 1;
        return 0;
    }

    public void OnLevelCompleted(float time)
    {
        completionTime = time;
        currentRating = CalculateRating(time);

        UpdateUI();
        ShowMenu();
    }

    public void NextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene <= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);            
        }
        HideMenu();

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        HideMenu();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        HideMenu();
    }
    private void UpdateUI()
    {
        if(timeText != null)
        {
            timeText.text  = $"Time = {completionTime:F2}s";
        }

        for(int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = (i < currentRating) ? filledStar : emptyStar;
        }
    }
}
