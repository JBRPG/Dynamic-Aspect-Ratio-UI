using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * This will adjust the Game Area's aspect ratio
 * And will also consider the camera's current angle
 * prior to adjusting the camera box itself
 * to match game aspect ratio after rotation.
 */

public class ScreenAspectAdjuster : MonoBehaviour
{

    private LetterBoxer letterBoxer;

    public void AdjustAspectRatioScreen()
    {
        letterBoxer.AdjustLetterBoxSizing();
    }
    public void AdjustAspectRatioScreen(Vector2 gameAspect)
    {
        letterBoxer.AdjustLetterBoxSizing(gameAspect);
    }

    // Start is called before the first frame update
    void Start()
    {
        letterBoxer = GetComponent<LetterBoxer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
