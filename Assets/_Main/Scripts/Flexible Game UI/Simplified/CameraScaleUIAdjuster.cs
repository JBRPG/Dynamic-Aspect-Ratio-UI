using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScaleUIAdjuster : MonoBehaviour
{
    [Header("Used as basis for width and height")]
    public float baseReferenceLength;

    public GridLayoutGroup uiScreen;
    public RectTransform canvasRect;
    public LetterBoxer letterBoxer;

    private float aspectRatio;

    public void AdjustUIFromAspectRatio()
    {
        SetupUIAspectRatio();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupUIAspectRatio();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * For the flexible aspect ratio to really work,
     * You need to consider the various scenarios:
     * 
     * When target game aspect ratio is same as display
     * Target game aspect ratio's width is less than display's aspect ratio width
     * Target game aspect ratio's height is taller than display's aspect ratio height
     */
    private void SetupUIAspectRatio()
    {
        Vector2 approxUIResolution = new Vector2()
        {
            x = Mathf.Round(canvasRect.rect.width),
            y = Mathf.Round(canvasRect.rect.height)
        };
        aspectRatio = approxUIResolution.x / approxUIResolution.y;
        float otherAspectLength = aspectRatio < 1 ?
            baseReferenceLength / aspectRatio :
            baseReferenceLength * aspectRatio;
        float rotationAngle = Mathf.Abs(uiScreen.transform.localEulerAngles.z);
        bool isRotated = (int)rotationAngle / 90 % 2 == 1;

        bool verticalGameScreen = letterBoxer.AspectRatio < 1;
        bool horizontalGameScreen = letterBoxer.AspectRatio > 1;

        if (verticalGameScreen)
        {
            uiScreen.cellSize = new Vector2(baseReferenceLength, otherAspectLength);
        }
        else if (horizontalGameScreen)
        {
            uiScreen.cellSize = new Vector2(otherAspectLength, baseReferenceLength);
        }

        CompareAspectRatioScaling(letterBoxer.AspectRatioDisplayScreen, letterBoxer.AspectRatio, isRotated);
        uiScreen.gameObject.GetComponent<ContentFitterRefresh>().RefreshContentFitters();


    }

    private void CompareAspectRatioScaling(float displayAspectRatio, float gameAspectRatio, bool isRotated)
    {
        Debug.Log("Screen Aspect Ratio (W/H): " + letterBoxer.AspectRatioDisplayScreen);
        Debug.Log("Game Aspect Ratio (W/H): " + letterBoxer.AspectRatio);

        // First calculate the display window / screen's aspect ratio
        float displayAspectRatioScale = displayAspectRatio < 1 ?
            displayAspectRatio: 1 / displayAspectRatio;
        // Then calculate the aspect ratio based camera view box rectangle
        float gameAspectRatioScale = gameAspectRatio < 1 ?
            gameAspectRatio : 1 / gameAspectRatio;

        bool bothHorizontal = displayAspectRatio >= 1 && gameAspectRatio >= 1;
        bool bothVertical = displayAspectRatio < 1 && gameAspectRatio < 1;

        if (isRotated)
        {
            if (bothHorizontal || bothVertical)
            {
                ScaleUIElements(gameAspectRatioScale);
            }
            else
            {
                if ((displayAspectRatio < (1 / gameAspectRatio)) ||
                    (displayAspectRatio > (1 / gameAspectRatio)))
                {
                    ScaleUIElements(gameAspectRatioScale);
                }
                else
                {
                    ScaleUIElements(displayAspectRatioScale);
                }
            }
        }
        else
        {
            if (FloatExtensions.Approximately(displayAspectRatio, gameAspectRatio, 0.01f))
            {
                ScaleUIElements(1);
            }
            else if (bothHorizontal)
            {
                if (displayAspectRatio < gameAspectRatio)
                {
                    ScaleUIElements(displayAspectRatioScale);
                }
                else
                {
                    ScaleUIElements(1);
                }
            }
            else if (bothVertical)
            {
                if (displayAspectRatio > gameAspectRatio)
                {
                    ScaleUIElements(displayAspectRatioScale);
                }
                else
                {
                    ScaleUIElements(1);
                }
            }
            else
            {
                ScaleUIElements(displayAspectRatioScale * gameAspectRatioScale);
            }
        }
    }

    private void ScaleUIElements(float scale)
    {
        uiScreen.transform.localScale = Vector3.one * scale;
    }
}
