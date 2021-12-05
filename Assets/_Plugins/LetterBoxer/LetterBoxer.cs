using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LetterBoxer : MonoBehaviour
{    
    public enum ReferenceMode { DesignedAspectRatio, OrginalResolution };
  
    public ReferenceMode referenceMode; 
    public float aspectWidth=16;
    public float aspectHeight=9;  
    public float resolutionWidth = 960;
    public float resolutionHeight = 540;
    public bool onAwake = true;
    public bool onUpdate = true;
    public bool consistentOrthographicView;
    public float targetOrthographicSize;

    // Get the game screen's target aspect ratio
    // whenever rotated or not
    public float AspectRatio
    {
        get
        {
            return aspectWidth / aspectHeight;
        }
    }

    public float AspectRatioDisplayScreen
    {
        get
        {
            return (float)Screen.width / (float)Screen.height;
        }
    }

    public float AspectRatioGameCameraBox
    {
        get
        {
            return aspectRatioCameraBox.x / aspectRatioCameraBox.y;
        }
    }

    public Camera AttachedCamera
    {
        get { return cam; }
    }

    private Camera cam;
    private Vector2 aspectRatioCameraBox;
    private Vector2 resolutionRatioCameraBox;

    public void AdjustLetterBoxSizing()
    {
        PerformSizing();
    }
    public void AdjustLetterBoxSizing(Vector2 gameAspectRatio)
    {
        aspectWidth = gameAspectRatio.x;
        aspectHeight = gameAspectRatio.y;
        PerformSizing();
    }

    public float getScaledHeight()
    {
        return CalculateScaleHeight();
    }

    public float getScaledWidth()
    {
        return 1f / CalculateScaleHeight();
    }

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
    }

    public void Awake()
    {
        // perform sizing if onAwake is set
        if(onAwake)
        {
            PerformSizing();
        }
    }

    public void Update()
    {
        // perform sizing if onUpdate is set
        if (onUpdate)
        {
            PerformSizing();
        }
    }

    private void OnValidate()
    {
        cam = GetComponent<Camera>();
        aspectWidth = Mathf.Max(1, aspectWidth);
        aspectHeight = Mathf.Max(1, aspectHeight);
        resolutionWidth = Mathf.Max(1, resolutionWidth);
        resolutionHeight = Mathf.Max(1, resolutionHeight);
        aspectRatioCameraBox = new Vector2
        {
            x = IsCameraRotated() ? aspectHeight : aspectWidth,
            y = IsCameraRotated() ? aspectWidth : aspectHeight,

        };
        resolutionRatioCameraBox = new Vector2
        {
            x = IsCameraRotated() ? resolutionHeight : resolutionWidth,
            y = IsCameraRotated() ? resolutionWidth : resolutionHeight,

        };
    }

    // based on logic here from http://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html
    private void PerformSizing()
    {
        OnValidate();

        // current viewport height should be scaled by this amount
        float scaleheight = CalculateScaleHeight();

        
        Rect rect = cam.rect;
        if (Mathf.Approximately(scaleheight, 1f))
        {
            rect.width = 1f;
            rect.height = 1f;
            rect.x = 0;
            rect.y = 0;

            cam.rect = rect;
        }
        // if scaled height is less than current height, add letterbox
        else if (scaleheight < 1.0f)
        {

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            cam.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;


            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            cam.rect = rect;
        }
        PerformTargetOrthographicSize();
    }

    private void PerformTargetOrthographicSize()
    {
        if (consistentOrthographicView)
        {
            cam.orthographicSize = AspectRatioGameCameraBox < 1 ?
                targetOrthographicSize / AspectRatioGameCameraBox : targetOrthographicSize;
        }
    }

    private float CalculateScaleHeight()
    {
        // calc based on aspect ratio
        float targetRatio = aspectRatioCameraBox.x / aspectRatioCameraBox.y;

        // recalc if using resolution as reference
        if (referenceMode == LetterBoxer.ReferenceMode.OrginalResolution)
        {
            targetRatio = resolutionRatioCameraBox.x / resolutionRatioCameraBox.y;
        }

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        return windowaspect / targetRatio;
    }

    private bool IsCameraRotated()
    {
        if (cam == null)
        {
            return false;
        }
        float rotationAngle = cam.transform.localEulerAngles.z;
        bool cameraRotated = (int)rotationAngle / 90 % 2 == 1;
        return cameraRotated;
    }
}
