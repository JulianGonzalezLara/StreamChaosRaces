using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpeedTutorMainMenuSystem
{
    public class MenuController : MonoBehaviour
    {
        [Header("Variables to save")]
        private float _volumeLevel;
        private int _qualityLevel;
        private bool _isFullScreen;
        private float _brightnessLevel;
        public int mainControllerSen = 4;

        [Header("Default Menu Values")]
        [SerializeField] private float defaultBrightness = 1;
        [SerializeField] private float defaultVolume = 0.5f;
        [SerializeField] private int defaultSen = 4;
        [SerializeField] private bool defaultInvertY = false;

        [Header("Confirmation Object")]
        [SerializeField] private GameObject confirmationPrompt = null;
        [SerializeField] private GameObject noSavedGameDialog = null;

        [Header("Controller Sensitivity")]
        [SerializeField] private Text controllerSenText = null;
        [SerializeField] private Slider controllerSenSlider = null;

        [Header("Brightness Setting")]
        [SerializeField] private Brightness brightnessEffect = null;
        [SerializeField] private Slider brightnessSlider = null;
        [SerializeField] private Text brightnessText = null;

        [Header("Volume Setting")]
        [SerializeField] private Text volumeText = null;
        [SerializeField] private Slider volumeSlider = null;

        [Header("Invert Y Toggle")]
        [SerializeField] private Toggle invertYToggle = null;

        [Header("Resolution Dropdowns")]
        public Dropdown resolutionDropdown;
        Resolution[] resolutions;

        [SerializeField] private Dropdown qualityDropdown;
        [SerializeField] private Toggle fullScreenToggle;

        [Header("Input Field configuracion Twitch")]
        public InputField username;
        public InputField token;
        public InputField channelName;

        [Header("Menus")]
        public GameObject menuConfigTwitch;
        public GameObject menu;
        public GameObject menuNewGame;

        /// <summary>
        /// BASIC CONTROLS
        /// </summary>
        public void ClickNewGameDialog()
        {
            menu.SetActive(false);
            menuNewGame.SetActive(true);
        }

        public void PlayCircuito1()
        {
            GameManager.Instance.setGameState(GameStates.CIRCUITO1);
        }

        public void PlayCircuito2()
        {
            GameManager.Instance.setGameState(GameStates.CIRCUITO2);
        }

        public void ExitButton()
        {
            Application.Quit();
        }

        public void LoadConfigTwitch()
        {
            GameManager.Instance.setConfigTwitch(username.text, token.text, channelName.text);
            menuConfigTwitch.SetActive(false);
            menu.SetActive(true);
        }

        public void OcultarConfig()
        {
            menuConfigTwitch.SetActive(false);
            menu.SetActive(true);
        }

        public void BackNewGame()
        {
            menuNewGame.SetActive(false);
            menu.SetActive(true);
        }

        public void LoadHelp()
        {
            GameManager.Instance.setGameState(GameStates.HELP);
        }

        public void Token()
        {
            Application.OpenURL("https://twitchapps.com/tmi/");
        }

        /// <summary>
        /// VOLUME SETTINGS
        /// </summary>
        /// <param name="volume"></param>
        public void SetVolume(float volume)
        {
            _volumeLevel = volume;
            volumeText.text = volume.ToString("0.0");
        }

        public void VolumeApply()
        {
            PlayerPrefs.SetFloat("masterVolume", _volumeLevel);
            AudioListener.volume = _volumeLevel;
            StartCoroutine(ConfirmationBox());
        }

        /// <summary>
        ///GRAPHICS SECTION
        /// </summary>
        private void Start()
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetBrightness(float brightness)
        {
            _brightnessLevel = brightness;
            brightnessText.text = brightness.ToString("0.0");
        }

        public void SetFullScreen(bool isFullscreen)
        {
            _isFullScreen = isFullscreen;
        }

        public void SetQuality(int qualityIndex)
        {
            _qualityLevel = qualityIndex;
        }

        public void GraphicsApply()
        {
            PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
            brightnessEffect.brightness = _brightnessLevel;

            PlayerPrefs.SetInt("masterQuality", _qualityLevel);
            QualitySettings.SetQualityLevel(_qualityLevel);

            PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
            Screen.fullScreen = _isFullScreen;

            StartCoroutine(ConfirmationBox());
        }

        /// <summary>
        /// GAMEPLAY SECTION
        /// </summary>
        /// <param name="sensitivity"></param>
        public void SetControllerSen(float sensitivity)
        {
            mainControllerSen = Mathf.RoundToInt(sensitivity);
            controllerSenText.text = sensitivity.ToString("0");
        }

        public void GameplayApply()
        {
            if (invertYToggle.isOn)
            {
                PlayerPrefs.SetInt("masterInvertY", 1);
                //Invert your mouse Y here
            }

            else if (!invertYToggle.isOn)
            {
                PlayerPrefs.SetInt("masterInvertY", 0);
                //Turn invert Y off
            }

            PlayerPrefs.SetFloat("masterSen", mainControllerSen);
            //Change your controller sensitivity here
            StartCoroutine(ConfirmationBox());
        }

        public void ResetButton(string GraphicsMenu)
        {
            if (GraphicsMenu == "Graphics")
            {
                //brightnessEffect.brightness = defaultBrightness;
                brightnessSlider.value = defaultBrightness;
                brightnessText.text = defaultBrightness.ToString("0.0");

                qualityDropdown.value = 1;
                QualitySettings.SetQualityLevel(1);

                fullScreenToggle.isOn = false;
                Screen.fullScreen = false;

                Resolution currentResolution = Screen.currentResolution;
                Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
                resolutionDropdown.value = resolutions.Length;
                GraphicsApply();
            }

            if (GraphicsMenu == "Audio")
            {
                AudioListener.volume = defaultVolume;
                volumeSlider.value = defaultVolume;
                volumeText.text = defaultVolume.ToString("0.0");
                VolumeApply();
            }

            if (GraphicsMenu == "Gameplay")
            {
                controllerSenText.text = defaultSen.ToString("0");
                controllerSenSlider.value = defaultSen;
                mainControllerSen = defaultSen;

                invertYToggle.isOn = false;

                GameplayApply();
            }
        }

        public IEnumerator ConfirmationBox()
        {
            confirmationPrompt.SetActive(true);
            yield return new WaitForSeconds(2);
            confirmationPrompt.SetActive(false);
        }

        private void ClickSound()
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
