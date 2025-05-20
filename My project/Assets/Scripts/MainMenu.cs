using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenuCanvas;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        optionsMenuCanvas.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene("Level-Intro");
    }
    public void SelectLevel()
    {
        SceneManager.LoadScene("Level Selection");
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Player Has Quit The Game");
    }
}
    