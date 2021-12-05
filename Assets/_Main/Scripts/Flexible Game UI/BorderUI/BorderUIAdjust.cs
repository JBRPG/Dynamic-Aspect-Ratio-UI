using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * The border background layer contains adjustable
 * letterbox and pillar box.
 * 
 * Whenever the gameplay and screen aspect ratio are the same,
 * do not use any borders.
 * 
 * Whenever the game aspect ratio is different from screen,
 * activate the border box UI elements.
 * 
 * Compare the gameplay vanvas, the gameplay camera,
 * and the background border canvas
 * 
 * One way to achieve it is with both canvases having
 * the same reference width and height
 * to account for UI and camera rotation
 * 
 * Pillar Box: Camera's view rectangle width less than 1
 * Letter Box: Camera's view rectangle height less than 1
 */

public class BorderUIAdjust : MonoBehaviour
{
    public Camera gameplayCamera;
    public GameObject BackgroundLetterBox;
    public GameObject BackgroundPillarBox;
    public Camera backgroundCamera;
    public CanvasScaler backgroundScaler;

    public float baseReferenceLength;

    private LayoutElement[] letterBoxElements;
    private LayoutElement[] pillarBoxElements;

    private RectTransform rotateableUI;
    private GridLayoutGroup gridUI;

    public void adjustBorderBox()
    {
        backgroundCamera.ResetAspect();
        float cameraRectWidth = gameplayCamera.rect.width;
        float cameraRectHeight = gameplayCamera.rect.height;

        // Set up boolean conditions for pillar and letter boxes
        // Due to the possiblity of rotating the entire display screen
        float uiRotation = rotateableUI.localRotation.eulerAngles.z;
        bool isScreenRotated = (int)(uiRotation / 90f) % 2 == 1;
        bool cameraRectHeightShrunk = cameraRectHeight < 1;
        bool cameraRectWidthShrunk = cameraRectWidth < 1;

        bool displayPillarMode = (cameraRectWidthShrunk && !isScreenRotated) ||
            (cameraRectHeightShrunk && isScreenRotated);
        bool displayLetterMode = (cameraRectHeightShrunk && !isScreenRotated) ||
            (cameraRectWidthShrunk && isScreenRotated);
        SetupUIBackgroundBorders(isScreenRotated);

        float adjustViewWidth = isScreenRotated ? cameraRectHeight : cameraRectWidth;
        float adjustViewHeight = isScreenRotated ? cameraRectWidth : cameraRectHeight;

        if (Mathf.Approximately(cameraRectWidth, cameraRectHeight))
        {
            BackgroundLetterBox.SetActive(false);
            BackgroundPillarBox.SetActive(false);
        }
        else if (displayPillarMode)
        {
            BackgroundLetterBox.SetActive(false);
            BackgroundPillarBox.SetActive(true);
            modifyBorderElements(adjustViewWidth, ref pillarBoxElements);
        }
        else if (displayLetterMode)
        {
            BackgroundLetterBox.SetActive(true);
            BackgroundPillarBox.SetActive(false);
            modifyBorderElements(adjustViewHeight, ref letterBoxElements);
        }
        gridUI.gameObject.GetComponent<ContentFitterRefresh>().RefreshContentFitters();
    }

    // Start is called before the first frame update
    void Start()
    {
        rotateableUI = GetComponent<RectTransform>();
        gridUI = GetComponent<GridLayoutGroup>();
        InitializeBorderLayoutElements();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeBorderLayoutElements()
    {
        letterBoxElements = BackgroundLetterBox.
            GetComponentsInChildren<LayoutElement>();
        pillarBoxElements = BackgroundPillarBox.
            GetComponentsInChildren<LayoutElement>();

        foreach (var element in letterBoxElements)
        {
            element.flexibleHeight = 0.1f;
        }
        foreach (var element in pillarBoxElements)
        {
            element.flexibleWidth = 0.1f;
        }
        adjustBorderBox();
    }

    private void modifyBorderElements(float screenFactor, ref LayoutElement[] borderElements)
    {
        float borderOccupation = (1f - screenFactor) / 2f;
        for (int i = 0; i < borderElements.Length; ++i)
        {
            var element = borderElements[i];
            float result = borderOccupation;
            if (i == 1)
            {
                result = screenFactor;
            }

            if (element.transform.parent.name.Contains("PillarBox"))
            {
                element.flexibleWidth = result;
            }
            else if (element.transform.parent.name.Contains("LetterBox"))
            {
                element.flexibleHeight = result;
            }
        }
    }

    private void SetupUIBackgroundBorders(bool gameScreenRotated)
    {
        if (backgroundCamera.aspect < 1)
        {
            backgroundScaler.matchWidthOrHeight = 0;
        }
        else
        {
            backgroundScaler.matchWidthOrHeight = 1;
        }

        var targetAspectRatio = backgroundCamera.aspect;
        targetAspectRatio = targetAspectRatio < 1 ? 1 / targetAspectRatio : targetAspectRatio;

        var pillarMode = (backgroundCamera.aspect < 1 && !gameScreenRotated) || (backgroundCamera.aspect > 1 && gameScreenRotated);
        var letterMode = (backgroundCamera.aspect > 1 && !gameScreenRotated) || (backgroundCamera.aspect < 1 && gameScreenRotated);

        if (pillarMode)
        {
            gridUI.cellSize = new Vector2(baseReferenceLength, baseReferenceLength * targetAspectRatio);
        }
        else if (letterMode)
        {
            gridUI.cellSize = new Vector2(baseReferenceLength * targetAspectRatio, baseReferenceLength);
        }
    }
}
