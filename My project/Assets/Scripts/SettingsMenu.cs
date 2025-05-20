using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Toggle fullscreenTog;
    public List<Resolution> resolutions = new List<Resolution>();
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;
    }

    void Update()
    {

    }
    public void Apply()
    {
        Screen.fullScreen = fullscreenTog.isOn;

    }
}
