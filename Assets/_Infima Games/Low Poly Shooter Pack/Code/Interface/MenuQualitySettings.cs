//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Quality Settings Menu.
    /// </summary>
    public class MenuQualitySettings : Element
    {
        #region FIELDS SERIALIZED
        
        [Title(label: "Settings")]

        [Tooltip("Canvas to play animations on.")]
        [SerializeField]
        private GameObject animatedCanvas;

        [Tooltip("Animation played when showing this menu.")]
        [SerializeField]
        private AnimationClip animationShow;

        [Tooltip("Animation played when hiding this menu.")]
        [SerializeField]
        private AnimationClip animationHide;

        [SerializeField] Slider volumeSlider;
        [SerializeField] Slider volumeMusicSlider;
        [SerializeField] TMP_Dropdown resolutionDropdown;

        [SerializeField] AudioMixer _Mixer;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip uiClick;
        [SerializeField] AudioClip uiHover;
        [SerializeField] AudioClip uiSpecial;

        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private TMP_InputField mouseSensitivity;
        [SerializeField] private Toggle fullscreenToggle;

        private const string MOUSE_PATH = "<Pointer>";
        private InputAction action;

        Resolution[] resolutions;

        #endregion

        #region FIELDS

        /// <summary>
        /// Animation Component.
        /// </summary>
        private Animation animationComponent;
        /// <summary>
        /// If true, it means that this menu is enabled and showing properly.
        /// </summary>
        private bool menuIsEnabled;

        /// <summary>
        /// Main Post Processing Volume.
        /// </summary>
        private PostProcessVolume postProcessingVolume;
        /// <summary>
        /// Scope Post Processing Volume.
        /// </summary>
        private PostProcessVolume postProcessingVolumeScope;

        /// <summary>
        /// Depth Of Field Settings.
        /// </summary>
        private DepthOfField depthOfField;

        #endregion

        #region UNITY

        private void Start()
        {
            //Hide pause menu on start.
            animatedCanvas.GetComponent<CanvasGroup>().alpha = 0;
            //Get canvas animation component.
            animationComponent = animatedCanvas.GetComponent<Animation>();

            //Find post process volumes in scene and assign them.
            postProcessingVolume = GameObject.Find("Post Processing Volume")?.GetComponent<PostProcessVolume>();
            postProcessingVolumeScope = GameObject.Find("Post Processing Volume Scope")?.GetComponent<PostProcessVolume>();

            //Get depth of field setting from main post process volume.
            if (postProcessingVolume != null)
                postProcessingVolume.profile.TryGetSettings(out depthOfField);

            action = inputActionAsset.FindAction("Look");
            mouseSensitivity.onValueChanged.AddListener(delegate { if (float.TryParse(mouseSensitivity.text, out float mouseSens)) { SetMouseSensitivity(mouseSens); }; });
            fullscreenToggle.onValueChanged.AddListener(delegate { SetFullscreen(fullscreenToggle.isOn); });

            #region Resolution

            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
                options.Add(option);
                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            #endregion

            LoadSensitivity();
            LoadFullscreen();
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetString("Fullscreen", isFullscreen.ToString());
        }

        public void LoadFullscreen()
        {
            if (PlayerPrefs.GetString("Fullscreen") == "True")
            {
                fullscreenToggle.isOn = true;
            }
            else
            {
                fullscreenToggle.isOn = false;
            }
        }

        #region MouseSens

        private static void SetScale(InputAction action, string bindingPathStart, Vector2 scale)
        {
            var bindings = action.bindings;
            for (var i = 0; i < bindings.Count; i++)
            {
                if (bindings[i].isPartOfComposite || !bindings[i].path.StartsWith(bindingPathStart)) continue;
                action.ApplyBindingOverride(i,
                    new InputBinding { overrideProcessors = $"ScaleVector2(x={scale.x},y={scale.y})" });
                return;
            }
        }

        public void SetMouseSensitivity(float _sens)
        {
            SetScale(action, MOUSE_PATH, new Vector2(_sens, _sens));

            PlayerPrefs.SetFloat("MouseSensitivity", _sens);
        }

        public void LoadSensitivity()
        {
            mouseSensitivity.text = PlayerPrefs.GetFloat("MouseSensitivity").ToString();
        }

        #endregion

        protected override void Tick()
        {
            //Switch. Fades in or out the menu based on the cursor's state.
            bool cursorLocked = characterBehaviour.IsCursorLocked();
            if (Keyboard.current.escapeKey.wasPressedThisFrame && gameObject.activeSelf)
            {
                //Hide.
                if (menuIsEnabled)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Shows the menu by playing an animation.
        /// </summary>

        private void Show()
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
            volumeMusicSlider.value = PlayerPrefs.GetFloat("VolumeMusic");

            //Enabled.
            menuIsEnabled = true;

            //Play Clip.
            animationComponent.clip = animationShow;
            animationComponent.Play();

            //Enable depth of field effect.
            if(depthOfField != null)
                depthOfField.active = true;

            Invoke("ChangeTime", .25f);
        }
        /// <summary>
        /// Hides the menu by playing an animation.
        /// </summary>
        private void Hide()
        {
            //Disabled.
            menuIsEnabled = false;

            //Play Clip.
            animationComponent.clip = animationHide;
            animationComponent.Play();

            //Disable depth of field effect.
            if(depthOfField != null)
                depthOfField.active = false;

            ChangeTime();
        }

        public void ChangeTime()
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        public void SetVolume(float _volume)
        {
            // Adjust volume
            AudioListener.volume = _volume;

            // Save volume
            PlayerPrefs.SetFloat("Volume", _volume);
        }        
        
        public void SetMusicVolume(float _volumeMusic)
        {
            // Adjust volume
            _Mixer.SetFloat("Music", _volumeMusic);

            // Save volume
            PlayerPrefs.SetFloat("VolumeMusic", _volumeMusic);
        }

        public void SetQuality(int _qualityIndex)
        {
            QualitySettings.SetQualityLevel(_qualityIndex);
        }

        public void SetResolution(int _resolutionIndex)
        {
            Resolution resolution = resolutions[_resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void UIClick()
        {
            audioSource.PlayOneShot(uiClick);
        }

        public void UIHover()
        {
            audioSource.PlayOneShot(uiHover);
        }

        public void UISpecial()
        {
            audioSource.PlayOneShot(uiSpecial);
        }


        public void Restart()
        {
            //Path.
            string sceneToLoad = SceneManager.GetActiveScene().path;
            
            #if UNITY_EDITOR
            //Load the scene.
            UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
            #else
            //Load the scene.
            SceneManager.LoadSceneAsync(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
            #endif
        }
        public void Quit()
        {
            //Quit.
            Application.Quit();
        }

        #endregion
    }
}