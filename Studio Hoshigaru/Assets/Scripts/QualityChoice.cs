using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualityChoice : MonoBehaviour
{
    static int tmp;

    public Dropdown Quality;

    public void Awake()
    {
        int t = PlayerPrefs.GetInt("QualitySetting");
        QualitySettings.SetQualityLevel(t);
        Quality.value = t;
    }
    public void SetQuality(int qualityIndex)
    {
        tmp = qualityIndex;
        QualitySettings.SetQualityLevel(tmp);
    }

    public void SaveQuality()
    {
        PlayerPrefs.SetInt("QualitySetting", tmp);
    }
}
