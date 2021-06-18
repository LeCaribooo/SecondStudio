using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    
    static float masterVolume = 1.0f;


    void Update()
    {
        AudioListener.volume = masterVolume;   
    }

    public void SetVolume(float vol)
    {
        masterVolume = vol;
    }
}
