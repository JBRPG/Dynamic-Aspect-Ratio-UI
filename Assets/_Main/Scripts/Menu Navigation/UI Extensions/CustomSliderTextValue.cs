using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/*
 * This will send custom string based on whole numbers
 * from the slider to any targeted UI text entity
 */

public class CustomSliderTextValue : MonoBehaviour
{
    public TMP_InputField targetText;
    public string[] customValues;

    public void setCustomValueText(Single value)
    {
        int targetIndex = (int)value;
        targetText.text = customValues[targetIndex];
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
