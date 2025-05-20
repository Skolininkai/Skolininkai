using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    public Toggle fullscreenTog;
    [Header("Resolution Settings")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private List<Resolution> filteredResolutions = new List<Resolution>();
    private int currentResolutionIndex = 0;
    [Header("Audio Settings")]
    [SerializeField] private AudioMixerGroup musicGr;
    [SerializeField] private AudioMixerGroup soundGr;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private TMP_Text musicValueText;
    [SerializeField] private TMP_Text soundValueText;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundVolumeKey = "SoundVolume";
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;
        InitializeResolutionDropdown();
        InitializeVolumeControls();
    }
    public void Apply()
    {
        Resolution selected = filteredResolutions[currentResolutionIndex];
        Screen.SetResolution(selected.width, selected.height, fullscreenTog.isOn, selected.refreshRate);
        PlayerPrefs.SetInt("ResolutionIndex", currentResolutionIndex);

        // Apply fullscreen
        Screen.fullScreen = fullscreenTog.isOn;
        PlayerPrefs.Save();

    }
    void InitializeResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        filteredResolutions.Clear();

        // Get and sort all resolutions
        var resolutions = Screen.resolutions
            .GroupBy(r => (r.width, r.height))               // Group by dimensions
            .Select(g => g.OrderByDescending(r => r.refreshRate).First()) // Take highest refresh rate
            .OrderBy(r => r.width).ThenBy(r => r.height)     // Sort by width then height
            .ToList();

        // Create options and find current resolution
        List<string> options = new List<string>();
        int currentSavedIndex = PlayerPrefs.GetInt("ResolutionIndex", -1);
        
        for (int i = 0; i < resolutions.Count; i++)
        {
            var resolution = resolutions[i];
            string option = $"{resolution.width}x{resolution.height} ({resolution.refreshRate}Hz)";
            options.Add(option);
            filteredResolutions.Add(resolution);

            // Check if current resolution matches
            if (Screen.currentResolution.width == resolution.width && 
                Screen.currentResolution.height == resolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Apply saved resolution if valid
        if (currentSavedIndex != -1 && currentSavedIndex < filteredResolutions.Count)
        {
            currentResolutionIndex = currentSavedIndex;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolutionIndex);
    }

    public void SetResolutionIndex(int index)
    {
        currentResolutionIndex = index;
    }
    void InitializeVolumeControls()
    {
        // Initialize Music controls
        musicSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, 100f);
        UpdateMusicVolume(musicSlider.value);

        // Initialize Sound controls
        soundSlider.value = PlayerPrefs.GetFloat(SoundVolumeKey, 100f);
        UpdateSoundVolume(soundSlider.value);

        // Add listeners in case they weren't set in the inspector
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        soundSlider.onValueChanged.AddListener(UpdateSoundVolume);
    }

    public void UpdateMusicVolume(float value)
    {
        musicValueText.text = Mathf.RoundToInt(value).ToString();
        SetMixerVolume(musicGr, "MusicVolume", value);
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
    }

    public void UpdateSoundVolume(float value)
    {
        soundValueText.text = Mathf.RoundToInt(value).ToString();
        SetMixerVolume(soundGr, "SoundVolume", value);
        PlayerPrefs.SetFloat(SoundVolumeKey, value);
    }

    private void SetMixerVolume(AudioMixerGroup group, string parameterName, float sliderValue)
    {
        // Convert 0-100 slider range to -80dB to 0dB
        float dB = (sliderValue * 0.8f) - 80f;
        group.audioMixer.SetFloat(parameterName, dB);
    }
        void OnDisable()
    {
        // Ensure settings are saved when menu is closed
        PlayerPrefs.Save();
    }
}
