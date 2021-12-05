using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustWidthHeightUI : MonoBehaviour
{
    public LetterBoxer letterBoxer;
    public CanvasScaler canvasScaler;

    public void AdjustUIMatchScale()
    {
        SetMatchingWidthOrHeight();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMatchingWidthOrHeight();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetMatchingWidthOrHeight()
    {

        bool horizontalGameScreen = letterBoxer.AspectRatio > 1;
        bool verticalGameScreen = letterBoxer.AspectRatio < 1;
        if (horizontalGameScreen)
        {
            canvasScaler.matchWidthOrHeight = 1;
        }
        else if (verticalGameScreen)
        {
            canvasScaler.matchWidthOrHeight = 0;
        }
    }
}
