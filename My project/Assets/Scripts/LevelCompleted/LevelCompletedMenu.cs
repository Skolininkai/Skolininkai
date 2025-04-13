using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletedMenu : MonoBehaviour
{
    private static LevelCompletedMenu instance;
    public GameObject menuUI;

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

    // Call this when level is completed
    public void OnLevelCompleted()
    {
        ShowMenu();
    }

    public void NextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene <= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
            HideMenu();
        }

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
}
