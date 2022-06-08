using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedTutorMainMenuSystem
{
    public class Init_LoadPreferences : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] private bool canUse = false;
        [SerializeField] private MenuController menuController;

        [Header("Brightness Setting")]
        [SerializeField] private Brightness brightnessEffect;
        [SerializeField] private Text brightnessText;
        [SerializeField] private Slider brightnessSlider;

        [Header("Volume Setting")]
        [SerializeField] private Text volumeText;
        [SerializeField] private Slider volumeSlider;

        [Header("Sensitivty Setting")]
        [SerializeField] private Text controllerText;
        [SerializeField] private Slider controllerSlider;

        [Header("Quality Level Setting")]
        [SerializeField] private Dropdown qualityLevelDropdown;

        [Header("Fullscreen Setting")]
        [SerializeField] private Toggle fullscreenToggle;

        [Header("Invert Y Setting")]
        [SerializeField] private Toggle invertYToggle;

        void Awake()
        {
            if (canUse)
            {
                if (PlayerPrefs.HasKey("masterQuality"))
                {
                    int localQuality = PlayerPrefs.GetInt("masterQuality");

                    qualityLevelDropdown.value = localQuality;
                    QualitySettings.SetQualityLevel(localQuality);
                }

                if (PlayerPrefs.HasKey("masterFullscreen"))
                {
                    int localFullscreen = PlayerPrefs.GetInt("masterFullscreen");

                    if (localFullscreen == 1)
                    {
                        Screen.fullScreen = true;
                        fullscreenToggle.isOn = true;
                    }
                    else
                    {
                        Screen.fullScreen = false;
                        fullscreenToggle.isOn = false;
                    }
                }

                //BRIGHTNESS
                if (brightnessEffect != null)
                {
                    if (PlayerPrefs.HasKey("masterBrightness"))
                    {
                        float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                        brightnessText.text = localBrightness.ToString("0.0");
                        brightnessSlider.value = localBrightness;
                        brightnessEffect.brightness = localBrightness;
                    }

                    else
                    {
                        menuController.ResetButton("Brightness");
                    }
                }

                //VOLUME
                if (PlayerPrefs.HasKey("masterVolume"))
                {
                    float localVolume = PlayerPrefs.GetFloat("masterVolume");

                    volumeText.text = localVolume.ToString("0.0");
                    volumeSlider.value = localVolume;
                    AudioListener.volume = localVolume;
                }
                else
                {
                    menuController.ResetButton("Audio");
                }

                //CONTROLLER SENSITIVITY
                if (PlayerPrefs.HasKey("masterSen"))
                {
                    float localSensitivity = PlayerPrefs.GetFloat("masterSen");

                    controllerText.text = localSensitivity.ToString("0");
                    controllerSlider.value = localSensitivity;
                    menuController.mainControllerSen = Mathf.RoundToInt(localSensitivity);
                }
                else
                {
                    menuController.ResetButton("Graphics");
                }

                //INVERT Y
                if (PlayerPrefs.HasKey("masterInvertY"))
                {
                    if (PlayerPrefs.GetInt("masterInvertY") == 1)
                    {
                        invertYToggle.isOn = true;

                    }

                    else
                    {
                        invertYToggle.isOn = false;
                    }
                }
            }
        }
    }
}
