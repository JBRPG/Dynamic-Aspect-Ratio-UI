using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUIFitCameraScreen : MonoBehaviour
{
    public Camera mainCamera;
    private RectTransform rotatableUI;
    private RectTransform parentRectTransform;

    public void rotateUIScreen(float degrees)
    {

        Vector2 canvasDimensions = parentRectTransform.rect.size;
        float rotationDifferenceAnchorOffset = (canvasDimensions.y - canvasDimensions.x) / 2;

        rotatableUI.localRotation = Quaternion.Euler(0, 0, degrees);
        bool isUiRotated = (int)(degrees / 90f) % 2 == 1;

        if (isUiRotated)
        {
            // Remember, this configuration is a must
            // for screen rotation to work:
            // offset min (- , +) and offset max (+ , -) 
            rotatableUI.offsetMin = new Vector2(-rotationDifferenceAnchorOffset, rotationDifferenceAnchorOffset);
            rotatableUI.offsetMax = new Vector2(rotationDifferenceAnchorOffset, -rotationDifferenceAnchorOffset);
        }
        else
        {
            rotatableUI.offsetMin = new Vector2(0, 0);
            rotatableUI.offsetMax = new Vector2(0, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rotatableUI = GetComponent<RectTransform>();
        parentRectTransform = rotatableUI.parent.GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
