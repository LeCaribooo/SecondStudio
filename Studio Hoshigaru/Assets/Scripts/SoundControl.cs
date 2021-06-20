using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    [Range(0.0f, 1.0f)]

    static float masterVolume = 1.0f;

    public Slider GetValue;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("soundValue"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("soundValue");
            Debug.LogWarning("Volume audio : " + AudioListener.volume);
            masterVolume = PlayerPrefs.GetFloat("soundValue");
            GetValue.value = masterVolume;
        }
    }

    void Update()
    {
        AudioListener.volume = masterVolume;   
    }

    public void SetVolume(float vol)
    {
        masterVolume = vol;
    }
}
