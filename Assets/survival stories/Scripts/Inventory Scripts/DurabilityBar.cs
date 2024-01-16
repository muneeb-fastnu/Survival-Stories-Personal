using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;

public class DurabilityBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Start()
    {
        SetMaxValue(1);
    }
    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
        slider.value = value;

        Color currentColor = gradient.Evaluate(1f);
        currentColor.a = 0.5f;
        fill.color = currentColor;
    }
    public void SetDurability(float value)
    {
        slider.value = value;
        Color currentColor = gradient.Evaluate(slider.normalizedValue);
        currentColor.a = 0.5f;
        fill.color = currentColor;
    }
    public void ButtonDamage()
    {
        SetDurability(slider.value - 0.2f);
    }
}
