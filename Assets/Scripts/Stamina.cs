using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxStamina(int Stamina)
    {
        slider.maxValue = Stamina;
        slider.value = Stamina;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetStamina(int Stamina)
    {
        slider.value = Stamina;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
