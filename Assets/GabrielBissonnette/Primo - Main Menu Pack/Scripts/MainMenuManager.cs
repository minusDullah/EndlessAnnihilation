using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

public class MainMenuManager : MonoBehaviour
{
    #region Variables

    [Header("On/Off")]
    [Space(5)] [SerializeField] bool showBackground;
    [SerializeField] bool showSocial1;
    [SerializeField] bool showSocial2;
    [SerializeField] bool showSocial3;
    [SerializeField] bool showVersion;
    [SerializeField] bool showFade;

    [Header("Scene")]
    [Space(10)] [SerializeField] string sceneToLoad;

    [Header("Sprites")]
    [Space(10)] [SerializeField] Sprite logo;
    [SerializeField] Sprite background;
    [SerializeField] Sprite buttons;

    [Header("Color")]
    [Space(10)] [SerializeField] Color32 mainColor;
    [SerializeField] Color32 secondaryColor;

    [Header("Version")]
    [Space(10)] [SerializeField] string version = "v.0105";

    [Header("Texts")]
    [Space(10)] [SerializeField] string play = "Play";
    [SerializeField] string settings = "Settings";
    [SerializeField] string quit = "Quit";

    [Header("Social")]
    [Space(10)] [SerializeField] Sprite social1Icon;
    [SerializeField] string social1Link;
    [Space(5)]
    [SerializeField] Sprite social2Icon;
    [SerializeField] string social2Link;
    [Space(5)]
    [SerializeField] Sprite social3Icon;
    [SerializeField] string social3Link;
    List<string> links = new List<string>();

    [Header("Audio")]
    [Space(10)] [SerializeField] float defaultVolume = 0.3f;
    [SerializeField] AudioClip uiClick;
    [SerializeField] AudioClip uiHover;
    [SerializeField] AudioClip uiSpecial;


    // Components
    [Header("Components")]
    [SerializeField] GameObject homePanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject bannerPanel;
    [SerializeField] Image social1Image;
    [SerializeField] Image social2Image;
    [SerializeField] Image social3Image;
    [SerializeField] Image logoImage;
    [SerializeField] Image backgroundImage;

    [Header("Fade")]
    [Space(10)] [SerializeField] Animator fadeAnimator;

    [Header("Color Elements")]
    [Space(5)] [SerializeField] Image[] mainColorImages;
    [SerializeField] TextMeshProUGUI[] mainColorTexts;
    [SerializeField] Image[] secondaryColorImages;
    [SerializeField] TextMeshProUGUI[] secondaryColorTexts;
    [SerializeField] Image[] buttonsElements;

    [Header("Texts")]
    [Space(10)] [SerializeField] TextMeshProUGUI playText;
    [SerializeField] TextMeshProUGUI settingsText;
    [SerializeField] TextMeshProUGUI quitText;
    [SerializeField] TextMeshProUGUI versionText;

    [Header("Settings")]
    [Space(10)] [SerializeField] Slider volumeSlider;
    [SerializeField] Slider volumeMusicSlider;
    [SerializeField] TMP_Dropdown resolutionDropdown;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioMixer _Mixer;

    Resolution[] resolutions;

    [SerializeField] private InputActionAsset inputActionAsset;
    [SerializeField] private float defaultSensitivity = 0.05f;
    [SerializeField] private TMP_InputField mouseSensitivity;
    [SerializeField] private Toggle fullscreenToggle;
    private const string MOUSE_PATH = "<Pointer>";
    private InputAction action;
    #endregion

    void Start()
    {
        SetStartUI();
        ProcessLinks();
        SetStartVolume();
        SetStartSensitivity();
        SetStartFullscreen();

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

    }

    #region Mouse Sensitivity

    private void Awake()
    {
        action = inputActionAsset.FindAction("Look");
        mouseSensitivity.onValueChanged.AddListener(delegate { if (float.TryParse(mouseSensitivity.text, out float mouseSens)) { SetMouseSensitivity(mouseSens); }; });
        //DebugBindings(action);
        //SetScale(action, MOUSE_PATH, new Vector2(5, 5));
        //DebugBindings(action);
    }

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

    /*
    private static void DebugBindings(InputAction action)
    {
        foreach (var binding in action.bindings.Where(binding => !binding.isPartOfComposite))
            Debug.Log($"{binding.path}, {binding.effectiveProcessors}");
    }
    */

    public void SetMouseSensitivity(float _sens)
    {
        SetScale(action, MOUSE_PATH, new Vector2(_sens, _sens));

        PlayerPrefs.SetFloat("MouseSensitivity", _sens);
    }

    void SetStartSensitivity()
    {
        if (!PlayerPrefs.HasKey("MouseSensitivity"))
        {
            PlayerPrefs.SetFloat("MouseSensitivity", defaultSensitivity);
            LoadSensitivity();
        }
        else
        {
            LoadSensitivity();
        }
    }

    public void LoadSensitivity()
    {
        mouseSensitivity.text = PlayerPrefs.GetFloat("MouseSensitivity").ToString();
        SetMouseSensitivity(float.Parse(mouseSensitivity.text));
    }

    #endregion

    private void SetStartUI()
    {
        //fadeAnimator.SetTrigger("FadeIn");
        homePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void UIEditorUpdate()
    {
        // Used to update the UI when not in play mode

        #region Sprites

        // Logo
        if (logoImage != null)
        {
            logoImage.sprite = logo;
            logoImage.color = mainColor;
            logoImage.SetNativeSize();
        }

        // Background
        if (backgroundImage != null)
        {
            backgroundImage.gameObject.SetActive(showBackground);
            backgroundImage.sprite = background;
            backgroundImage.SetNativeSize();
        }

        // Main Color Images
        for (int i = 0; i < mainColorImages.Length; i++)
        {
            mainColorImages[i].color = mainColor;
        }

        // Main Color Texts
        for (int i = 0; i < mainColorTexts.Length; i++)
        {
            mainColorTexts[i].color = mainColor;
        }

        // Secondary Color Images
        for (int i = 0; i < secondaryColorImages.Length; i++)
        {
            secondaryColorImages[i].color = secondaryColor;
        }

        // Secondary Color Texts
        for (int i = 0; i < secondaryColorTexts.Length; i++)
        {
            secondaryColorTexts[i].color = secondaryColor;
        }

        // Buttons Elements
        for (int i = 0; i < buttonsElements.Length; i++)
        {
            buttonsElements[i].sprite = buttons;
        }

        // Fade
        fadeAnimator.gameObject.SetActive(showFade);

        #endregion


        #region Texts

        if (playText != null)
            playText.text = play;

        if (settingsText != null)
            settingsText.text = settings;

        if (quitText != null)
            quitText.text = quit;

        // Version number
        versionText.gameObject.SetActive(showVersion);
        if (versionText != null)
            versionText.text = version;

        #endregion


        #region Social

        if (social1Image != null)
        {
            social1Image.sprite = social1Icon;
            social1Image.gameObject.SetActive(showSocial1);
        }

        if (social2Image != null)
        {
            social2Image.sprite = social2Icon;
            social2Image.gameObject.SetActive(showSocial2);
        }

        if (social3Image != null)
        {
            social3Image.sprite = social3Icon;
            social3Image.gameObject.SetActive(showSocial3);
        }

        #endregion
    }

    #region Links
    public void OpenLink(int _index)
    {
        if (links[_index].Length > 0)
            Application.OpenURL(links[_index]);
    }

    private void ProcessLinks()
    {
        if (social1Link.Length > 0)
            links.Add(social1Link);

        if (social2Link.Length > 0)
            links.Add(social2Link);

        if (social3Link.Length > 0)
            links.Add(social3Link);
    }
    #endregion




    public void Quit()
    {
        Application.Quit();
    }


    #region Audio

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

    void SetStartVolume()
    {
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", defaultVolume);
            LoadVolume();
        }
        else
        {
            LoadVolume();
        }

        if (!PlayerPrefs.HasKey("VolumeMusic"))
        {
            PlayerPrefs.SetFloat("VolumeMusic", -15f);
            LoadVolume();
        }
        else
        {
            LoadVolume();
        }
    }

    public void triggerFade()
    {
        fadeAnimator.SetTrigger("FadeOut");
    }

    public void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        volumeMusicSlider.value = PlayerPrefs.GetFloat("VolumeMusic");
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

    #endregion


    #region Graphics & Resolution Settings

    public void SetQuality(int _qualityIndex)
    {
        QualitySettings.SetQualityLevel(_qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        fullscreenToggle.isOn = isFullscreen;
        PlayerPrefs.SetString("Fullscreen", isFullscreen.ToString());
    }

    void SetStartFullscreen()
    {
        if (!PlayerPrefs.HasKey("Fullscreen"))
        {
            PlayerPrefs.SetString("Fullscreen", "True");
            fullscreenToggle.isOn = true;
        }
        else
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

        Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    #endregion
}