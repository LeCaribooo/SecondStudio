using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundValue : MonoBehaviour
{

    public Slider slider;

    void Update()
    {
        this.GetComponent<Text>().text = (int)(slider.value * 100f) + "%";
    }
}
