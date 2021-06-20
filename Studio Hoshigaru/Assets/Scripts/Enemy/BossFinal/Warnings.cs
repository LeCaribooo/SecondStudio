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
    public MainBoss boss;
    public int type;

    public void Start()
    {
        light = L.GetComponent<Light2D>();
    }

    void Update()
    {
        if(warn)
        { 
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
                if(type == 1)
                {
                    boss.TentacleSpawn(transform.position);
                }
                else
                {
                    boss.MeteorSpawn(transform.position);
                }
            }
            else if(light.intensity >= 1f)
            {
                middle = true;
            }
        }
    }
}
