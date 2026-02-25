using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using TMPro;

/// <summary>
/// Acts as the presenter (MVP pattern) for the settings UI
/// Will listen to OnChange events from UI elements of settings panel, then settings values of Settings
/// </summary>
public class SettingsUI : MonoBehaviour
{

    [SerializeField] Slider volumeSlider;
    [SerializeField] TMP_InputField volumeInput;
    float minVolume = 0;
    float maxVolume = 2;

    [SerializeField] Slider sensitivitySlider;
    [SerializeField] TMP_InputField sensitivityInput;
    float minSensitivity = 0.01f;
    float maxSensitivity = 2;

    [SerializeField] Settings settings;


    private void Awake()
    {
        volumeSlider.minValue = minVolume;
        volumeSlider.maxValue = maxVolume;

        sensitivitySlider.minValue = minSensitivity;
        sensitivitySlider.maxValue = maxSensitivity;

        UpdateUI();
    }

    private void OnEnable()
    {
        volumeSlider?.onValueChanged.AddListener(delegate { SetVolume(volumeSlider.value); });
        volumeInput?.onEndEdit.AddListener(delegate { SetVolume(float.Parse(volumeInput.text)); });

        sensitivitySlider?.onValueChanged.AddListener(delegate { SetSensitivity(sensitivitySlider.value); });
        sensitivityInput?.onEndEdit.AddListener(delegate { SetSensitivity(float.Parse(sensitivityInput.text)); });
    }

    private void OnDisable()
    {
        volumeSlider?.onValueChanged.RemoveListener(delegate { SetVolume(volumeSlider.value); });
        volumeInput?.onEndEdit.RemoveListener(delegate { SetVolume(float.Parse(volumeInput.text)); });

        sensitivitySlider?.onValueChanged.RemoveListener(delegate { SetSensitivity(sensitivitySlider.value); });
        sensitivityInput?.onEndEdit.RemoveListener(delegate { SetSensitivity(float.Parse(sensitivityInput.text)); });
    }

    /// <summary>
    /// Sets volume data in Settings and updates UI
    /// </summary>
    /// <param name="volume"></param>
    private void SetVolume(float volume)
    {
        if (volume < minVolume) volume = 0;
        if (volume > maxVolume) volume = maxVolume;

        settings.volume = Mathf.Round(volume * 100) / 100;
        settings.OnSettingsChanged.Invoke();
        UpdateUI();
    }

    /// <summary>
    /// Sets sensitivity data in Settings and updates UI
    /// </summary>
    /// <param name="sensitivity"></param>
    private void SetSensitivity(float sensitivity)
    {
        if (sensitivity < minSensitivity) sensitivity = 0;
        if (sensitivity > maxSensitivity) sensitivity = maxSensitivity;

        settings.sensitivity = Mathf.Round(sensitivity * 100) / 100;
        settings.OnSettingsChanged.Invoke();
        UpdateUI();
    }

    /// <summary>
    /// Updates UI according to settings values
    /// </summary>
    private void UpdateUI()
    {
        Debug.Log("Vol: " + settings.volume + "Sens: " + settings.sensitivity);
        volumeInput.text = settings.volume.ToString();
        volumeSlider.value = settings.volume;

        sensitivityInput.text = settings.sensitivity.ToString();
        sensitivitySlider.value = settings.sensitivity;
    }


}
