using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Keep in mind that the game board / map is often not a child of the camera in hierarchy.
 * 
 * If you do consider rotating the game map and you have entities moving around,
 * it is best to alter the movement controls based on screen rotation,
 * as in translate movement controls by the following:
 * -- move direction + rotate by (game screen rotation degrees) --
 * since they may not behave the same way as the user interface.
 */

public class RotateGameBoard : MonoBehaviour
{

    public void rotateCameraButton(float degrees)
    {
        transform.localRotation = Quaternion.Euler(0, 0, degrees);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
