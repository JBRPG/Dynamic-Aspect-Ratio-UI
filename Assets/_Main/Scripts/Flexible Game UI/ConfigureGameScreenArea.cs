using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigureGameScreenArea : MonoBehaviour
{
    public CameraScaleUIRotator cameraUiMainRotator;
    public CameraScaleUIRotator cameraUiBackgroundRotator;
    public BorderUIAdjust backgroundBorderUiAdjust;
    public ScreenAspectAdjuster screenAspectAdjuster;
    public AdjustWidthHeightUI adjustWidthHeightUI;
    public CameraScaleUIAdjuster cameraScaleMainUIAdjuster;

    public void AdjustGameScreenArea(float angle, Vector2 gameAspectRatio)
    {
        StartCoroutine(adjustGameScreenSettingsRoutine(angle, gameAspectRatio));
    }

    private static ConfigureGameScreenArea _instance;

    public static ConfigureGameScreenArea Instance { get { return _instance; } }

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

    private IEnumerator adjustGameScreenSettingsRoutine(float angle, Vector2 gameAspectRatio)
    {
        
        cameraUiMainRotator.RoatateGameScreen(angle);
        yield return new WaitForEndOfFrame();
        screenAspectAdjuster.AdjustAspectRatioScreen(gameAspectRatio);
        yield return new WaitForEndOfFrame();
        adjustWidthHeightUI.AdjustUIMatchScale();
        yield return new WaitForEndOfFrame();
        cameraScaleMainUIAdjuster.AdjustUIFromAspectRatio();
        yield return new WaitForEndOfFrame();
        cameraUiBackgroundRotator.RoatateGameScreen(angle);
        yield return new WaitForEndOfFrame();
        backgroundBorderUiAdjust.adjustBorderBox();
        yield return new WaitForEndOfFrame();

    }
}
