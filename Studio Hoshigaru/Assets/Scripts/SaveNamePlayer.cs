using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveNamePlayer : MonoBehaviour
{

    public InputField InputField;

    public void Awake()
    {
        InputField.text = PlayerPrefs.GetString("SaveName");
    }
    public void profileName()
    {
        PlayerPrefs.SetString("SaveName", InputField.text);
    }
}
