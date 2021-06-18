using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlide : MonoBehaviour
{
    public Slider slide;

    private void Awake()
    {
        slide.value = AudioListener.volume;
    }
}
