using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateScreenButton : MonoBehaviour
{
    public CameraScaleUIRotator cameraUiMainRotator;
    public CameraScaleUIRotator cameraUiBackgroundRotator;
    public BorderUIAdjust backgroundBorderUiAdjust;
    public ScreenAspectAdjuster screenAspectAdjuster;
    public AdjustWidthHeightUI adjustWidthHeightUI;
    public CameraScaleUIAdjuster cameraScaleMainUIAdjuster;

    public void RotateScreen(float angle)
    {
        Debug.Log("Rotate Screen Button is clicked.");
        cameraUiMainRotator.RoatateGameScreen(angle);
        screenAspectAdjuster.AdjustAspectRatioScreen();
        adjustWidthHeightUI.AdjustUIMatchScale();
        cameraScaleMainUIAdjuster.AdjustUIFromAspectRatio();
        cameraUiBackgroundRotator.RoatateGameScreen(angle);
        backgroundBorderUiAdjust.adjustBorderBox();

    }
}
