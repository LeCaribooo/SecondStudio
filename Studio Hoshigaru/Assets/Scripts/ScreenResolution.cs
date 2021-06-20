using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResolution : MonoBehaviour
{
    static bool tmp;

    public Toggle full;
    
    public Dropdown dropdown;
    
    Resolution[] resolutions;

    private void Awake()
    {
        bool foule = PlayerPrefs.GetInt("full") == 1;
        tmp = foule;
        full.isOn = foule;
        SetFullScreen(foule);
    }

    private void Start()
    {
        resolutions = Screen.resolutions;

        dropdown.ClearOptions();

        List<string> option = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string options = resolutions[i].width + " x " + resolutions[i].height;
            option.Add(options);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        dropdown.AddOptions(option);
        dropdown.value = currentResolutionIndex;
        dropdown.RefreshShownValue();
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    
    public void SetFullScreen(bool isFull)
    {
        tmp = isFull;
        Screen.fullScreen = tmp;
        
    }

    public void SaveScreen()
    {
        if (tmp)
            PlayerPrefs.SetInt("full", 1);
        else
            PlayerPrefs.SetInt("full", 0);
    }
}
