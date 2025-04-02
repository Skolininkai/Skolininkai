using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class LevelSelect : MonoBehaviour
{
    public void LoadMain()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void LoadIntro()
    {
        SceneManager.LoadScene("Level-Intro");
    }
    public void Load1()
    {
        SceneManager.LoadScene("Level-1");
    }
    public void Load2()
    {
        SceneManager.LoadScene("Level-2");
    }
    public void Load3()
    {
        SceneManager.LoadScene("Level-3");
    }
}