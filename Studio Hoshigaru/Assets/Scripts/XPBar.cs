using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Text level;

    public void SetMaxXP(int maxp, int actualxp)
    {
        slider.maxValue = maxp;
        slider.value = actualxp;
    }

    public void SetXP(int xp)
    {
        slider.value = xp;
    }

    public void SetLevel(int level)
    {
        this.level.text = level.ToString();
    }
}
