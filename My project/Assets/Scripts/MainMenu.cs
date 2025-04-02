using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
    