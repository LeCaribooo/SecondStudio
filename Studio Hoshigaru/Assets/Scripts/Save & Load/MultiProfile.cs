using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiProfile : MonoBehaviour
{
    private static string currentProfile; //Layer de selection
    private static string playerName;
  
    public void currentP(string cp)
    {
        currentProfile = cp;
        Debug.LogWarning("/!\\ CURRENT PROFILE : " + currentProfile + " /!\\");
    }


    //Surcharge des opérateurs 
    
    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(currentProfile + key);
    }

    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(currentProfile + key);
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(currentProfile + key);
    }

    public static void SetInt(string key, int defaultValue)
    {
        PlayerPrefs.SetInt(currentProfile  + key, defaultValue);
    }

    public static void SetFloat(string key, float defaultValue)
    {
        PlayerPrefs.SetFloat(currentProfile  + key, defaultValue);
    }

    public static void SetString(string key, string defaultValue)
    {
        PlayerPrefs.SetString(currentProfile  + key, defaultValue);
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(currentProfile + key);
    }
}
