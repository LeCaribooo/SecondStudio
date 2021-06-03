using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] Text level;

    public void SetMaxXP(int maxp, int actualxp)
    {
        slider.maxValue = maxp;
        slider.value = actualxp;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetXP(int xp)
    {
        slider.value = xp;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetLevel(int level)
    {
        this.level.text = level.ToString();
    }
}
