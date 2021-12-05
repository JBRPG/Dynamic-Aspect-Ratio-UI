using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScaleUIRotator : MonoBehaviour {

    public GameObject targetCamera;
    public GridLayoutGroup uiScreen;
    public RectTransform canvasRect;

    public void RoatateGameScreen(float angle)
    {
        SetupGameRotation(angle);
    }

    private void Start()
    {
        SetupGameRotation(0);
    }

    private void SetupGameRotation(float angle)
    {
        targetCamera.GetComponent<Camera>().ResetAspect();
        targetCamera.transform.localEulerAngles = new Vector3(0f, 0f, -angle);
        uiScreen.transform.localEulerAngles = new Vector3(0f, 0f, angle);
        
    }
}
