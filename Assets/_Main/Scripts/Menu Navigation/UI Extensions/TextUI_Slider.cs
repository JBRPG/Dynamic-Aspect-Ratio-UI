using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextUI_Slider : MonoBehaviour
{
    public TMP_Text sliderText;

    private Slider slider;

    public void selectColor()
    {
        sliderText.color = slider.colors.selectedColor;
    }

    public void normalColor()
    {
        sliderText.color = slider.colors.normalColor;
    }

    public void changeValueFromSlider(System.Single val)
    {
        sliderText.text = val.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        sliderText.text = slider.value.ToString();
    }
}
