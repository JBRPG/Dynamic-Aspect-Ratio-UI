using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;

/*
 * This will store series of settings data to then be used
 * to affect the game settings
*/

public class GameSettingsData : MonoBehaviour
{

    private static GameSettingsData _instance;

    public static GameSettingsData Instance { get { return _instance; } }

    // Game Display options
    public float GameAreaAspectRatio { get; private set; }
    public float GameAreaRotationAngle { get; private set; }
    public float DisplayAspectRatio { get; private set; }
    public Vector2Int DisplayResolution { get; private set; }

    public TMP_Dropdown gameAspectRatioDropDown;
    public TMP_Dropdown gameAreaRotationDropDown;
    public TMP_Dropdown displayAspectRatioDropDown;
    public TMP_Dropdown displayResolutionDropDown;
    public Toggle changedAspectRatioGame;
    public Toggle changedRotationGame;
    public Toggle changedAspectRatioDisplay;
    public Toggle changedResolutionDisplay;

    private Vector2 currentGameAspectRatio;
    private float currentGameScreenRotation;
    private Vector2 currentDisplayAspectRatio;
    private int currentDisplayResolutionLength;
    private bool fullScreenToggle;

    /// <summary>
    /// These are the following aspect ratios used
    /// game screen
    /// </summary>
    private readonly List<Vector2> aspectRatiosGameScreen = new List<Vector2>
    {
        new Vector2(4,3),
        new Vector2(3,4),
        new Vector2(16,9),
        new Vector2(9,16),
    };

    /// <summary>
    /// These are the following aspect ratios used
    /// for display screen / window
    /// </summary>
    private readonly List<Vector2> aspectRatiosDisplayWindow = new List<Vector2>
    {
        new Vector2(4,3),
        new Vector2(3,4),
        new Vector2(16,9),
        new Vector2(9,16),
    };

    /// <summary>
    /// Use the resolution 'height' length based on horizontal displays.
    ///
    /// Calculate the following depending on orientation:
    ///
    ///
    /// Horizontal (Aspect Width >= Height):
    ///     resWidth = resLength * (aspect width / height)
    ///     resHeight = resLength
    ///
    /// Vertical (Aspect Width < Height):
    ///     resWidth = resLength
    ///     resHeight = resLength * (aspect height / width)
    /// 
    /// </summary>
    private readonly List<int> resolutionLengthValues = new List<int>
    {
        360,
        480,
        576,
        600,
        648,
        720,
        768,
        900,
        960,
        1080,
        1140,
        2160,
    };

    /// <summary>
    /// Used only for game screen, as it affects orientation
    /// of the game camera and UI, along with the background border's
    /// camera and UI
    /// </summary>
    private readonly List<float> screenRotationAngles = new List<float>
    {
        0,
        90,
        180,
        270,
    };

    public void SetGameAspectRatioDropDown(int dropdownValue)
    {
        currentGameAspectRatio = aspectRatiosGameScreen[dropdownValue];

        changedAspectRatioGame.isOn = GameAreaAspectRatio != ObtainAspectRatioFloat(currentGameAspectRatio);
    }

    public void SetGameScreenRotationDropDown(int dropdownValue)
    {
        currentGameScreenRotation = screenRotationAngles[dropdownValue];
        changedRotationGame.isOn = GameAreaRotationAngle != currentGameScreenRotation;
    }

    public void SetDisplayAspectRatioDropDown(int dropdownValue)
    {
        currentDisplayAspectRatio = aspectRatiosDisplayWindow[dropdownValue];
        StartCoroutine(ChangeDisplayResolutionOptionsCoroutine());
        changedAspectRatioDisplay.isOn = DisplayAspectRatio != ObtainAspectRatioFloat(currentDisplayAspectRatio);
    }

    public void SetDisplayResolutionDropDown(int dropdownValue)
    {
        currentDisplayResolutionLength = resolutionLengthValues[dropdownValue];
        changedResolutionDisplay.isOn = !DisplayResolution.Equals(ObtainResolution(currentDisplayResolutionLength));

    }

    public void ToggleFullScreenMode(bool setToggle)
    {
        fullScreenToggle = setToggle;
    }

    /// <summary>
    /// Once apply button is made, perform the following order:
    ///
    /// 
    /// Check whether full screen is used or not to go full screen or window
    /// Change the display screen resolution
    /// Rotate the game screen camera and UI
    /// change aspect ratio on game camera
    /// Adjust size of game UI from adjusted aspect ratio
    /// Rotate the border area camera and UI
    /// Adjust border type based on rotated game screen
    ///
    /// Finally,
    /// Add any game specific touches, like background images
    /// and / or additional in game info gadgets
    /// </summary>
    public void ApplyChanges()
    {
        InitializePublicData();
        VerifyChangedSettings();
        StartCoroutine(ApplyDisplayResolutionRoutine());
        StartCoroutine(ApplyGameScreenAreaRoutine());
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitilaizeDisplayOptionsCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeData()
    {
        currentGameAspectRatio = aspectRatiosGameScreen[0];
        currentGameScreenRotation = screenRotationAngles[0];
        currentDisplayAspectRatio = aspectRatiosDisplayWindow[0];
        currentDisplayResolutionLength = resolutionLengthValues[0];
    }

    private void InitializePublicData()
    {
        GameAreaAspectRatio = currentGameAspectRatio.x / currentGameAspectRatio.y;
        GameAreaRotationAngle = currentGameScreenRotation;
        DisplayAspectRatio = currentDisplayAspectRatio.x / currentDisplayAspectRatio.y;
        DisplayResolution = ObtainResolution(currentDisplayResolutionLength);
    }

    private void InitializeDropDownValues()
    {
        InitializeOptionsOnDropdown(aspectRatiosGameScreen,gameAspectRatioDropDown);
        InitializeOptionsOnDropdown(screenRotationAngles,gameAreaRotationDropDown);
        InitializeOptionsOnDropdown(aspectRatiosDisplayWindow,displayAspectRatioDropDown);
        InitializeOptionsOnDropdownDisplayResolution(resolutionLengthValues,displayResolutionDropDown);
        VerifyChangedSettings();
    }

    private void InitializeOptionsOnDropdown(List<float> items, TMP_Dropdown targetDropdown)
    {
        targetDropdown.options.Clear();
        foreach(var item in items)
        {
            var optionData = new OptionData(item.ToString("F0"));
            targetDropdown.options.Add(optionData);
        }
        var targetLabel = targetDropdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        targetLabel.text = targetDropdown.options[0].text;
    }

    private void InitializeOptionsOnDropdown(List<Vector2> items, TMP_Dropdown targetDropdown)
    {
        targetDropdown.options.Clear();
        foreach (var item in items)
        {
            var optionData = new OptionData(item.ToString("F0"));
            targetDropdown.options.Add(optionData);
        }
        var targetLabel = targetDropdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        targetLabel.text = targetDropdown.options[0].text;
    }

    private void InitializeOptionsOnDropdownDisplayResolution(List<int> items, TMP_Dropdown targetDropdown)
    {
        var storedValue = targetDropdown.value;
        targetDropdown.options.Clear();
        foreach (var item in items)
        {
            var resolution = ObtainResolution(item);

            var optionData = new OptionData(resolution.ToString());
            targetDropdown.options.Add(optionData);
        }
        targetDropdown.value = storedValue;
        var targetLabel = targetDropdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        targetLabel.text = targetDropdown.options[targetDropdown.value].text;
    }

    private void VerifyChangedSettings()
    {
        changedAspectRatioGame.isOn = GameAreaAspectRatio != ObtainAspectRatioFloat(currentGameAspectRatio);
        changedRotationGame.isOn = GameAreaRotationAngle != currentGameScreenRotation;
        changedAspectRatioDisplay.isOn = DisplayAspectRatio != ObtainAspectRatioFloat(currentDisplayAspectRatio);
        changedResolutionDisplay.isOn = !DisplayResolution.Equals(ObtainResolution(currentDisplayResolutionLength));
    }

    private void ApplyDisplayResolution()
    {
        // Check if full screen.
        // If true, enable full screen

        // Then calculate the aspect ratio of display
        // based in width and height
        // to display the resolution in pixels
        var resolution = ObtainResolution(currentDisplayResolutionLength);
        Screen.SetResolution(resolution.x, resolution.y, fullScreenToggle);
    }

    private void ApplyGameScreenArea()
    {
        // After configuring the display reoslution
        // Then set up the game screen to
        // set up aspect ratio of game screen and UI
        // along with matching rotation
        // and adjusting the background border area
        ConfigureGameScreenArea.Instance.AdjustGameScreenArea(
            currentGameScreenRotation,
            currentGameAspectRatio);

    }

    private Vector2Int ObtainResolution(int resolutionLength)
    {
        float aspectRatio = currentDisplayAspectRatio.x / currentDisplayAspectRatio.y;
        int otherResolutionLength;
        Vector2Int result;
        if (aspectRatio < 1)
        {
            otherResolutionLength = Mathf.RoundToInt(resolutionLength / aspectRatio);
            result = new Vector2Int(resolutionLength, otherResolutionLength);
        }
        else
        {
            otherResolutionLength = Mathf.RoundToInt(resolutionLength * aspectRatio);
            result = new Vector2Int(otherResolutionLength, resolutionLength);
        }
        return result;
    }

    private float ObtainAspectRatioFloat(Vector2 aspectRatio)
    {
        return aspectRatio.x / aspectRatio.y;
    }

    private IEnumerator InitilaizeDisplayOptionsCoroutine()
    {
        InitializeData();
        InitializePublicData();
        InitializeDropDownValues();
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator ChangeDisplayResolutionOptionsCoroutine()
    {
        InitializeOptionsOnDropdownDisplayResolution(resolutionLengthValues, displayResolutionDropDown);
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator ApplyDisplayResolutionRoutine()
    {
        ApplyDisplayResolution();
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator ApplyGameScreenAreaRoutine()
    {
        yield return new WaitForEndOfFrame();
        ApplyGameScreenArea();
    }
}
