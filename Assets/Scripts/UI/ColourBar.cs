using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourBar : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    //Used to update the colour value and slider automatically
    public void SetColour(float colour)
    {
        slider.value = colour;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    //Sets the max value of colour the slider can go to
    public void SetMaxColour(float colour)
    {
        slider.maxValue = colour;

        slider.value = colour;

        fill.color = gradient.Evaluate(1f);
    }
}
