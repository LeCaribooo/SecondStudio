using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Warnings : MonoBehaviour
{
    public bool warn;
    public bool middle;
    public GameObject L;
    public Light2D light;
    public bool end;

    public void Start()
    {
        light = L.GetComponent<Light2D>();
    }

    void Update()
    {
        if(warn)
        {
            if(end)
            {
                end = false;
            }
            if(!middle)
            {
                light.intensity += 0.005f;
            }
            else
            {
                light.intensity -= 0.005f;
            }
            if(light.intensity <= 0 && middle)
            {
                light.intensity = 0f;
                middle = false;
                warn = false;
                end = true;
            }
            else if(light.intensity >= 1f)
            {
                middle = true;
            }
        }
    }
}
