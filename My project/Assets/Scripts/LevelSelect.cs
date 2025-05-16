using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Codice.Client.Common.GameUI;
using TMPro;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{

    [Header("Star Rating")]
    public Image[] starImages;
    public Sprite emptyStar;
    public Sprite filledStar;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        FetchRatings();
    }

    private void FetchRatings()
    {
        int level = 2;
        for (int i = 0; i < 7 * 3; i += 3)
        {
            starImages[i].sprite = (0 < PlayerPrefs.GetInt($"Level_{level}_Rating", 0)) ? filledStar : emptyStar;
            starImages[i + 1].sprite = (1 < PlayerPrefs.GetInt($"Level_{level}_Rating", 0)) ? filledStar : emptyStar;
            starImages[i + 2].sprite = (2 < PlayerPrefs.GetInt($"Level_{level}_Rating", 0)) ? filledStar : emptyStar;
            level++;
        }
    }

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
    public void Load4()
    {
        SceneManager.LoadScene("Level-4");
    }
    public void Load5()
    {
        SceneManager.LoadScene("Level-5");
    }
    public void Load6()
    {
        SceneManager.LoadScene("Level-6");
    }
}