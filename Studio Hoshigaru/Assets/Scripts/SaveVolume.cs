using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveVolume : MonoBehaviour
{

    public Slider slidevalue;

    public void saveProfile()
    {
        PlayerPrefs.SetFloat("soundValue", slidevalue.value);
    }
}
