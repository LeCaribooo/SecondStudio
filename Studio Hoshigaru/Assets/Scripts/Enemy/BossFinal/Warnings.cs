using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Photon.Pun;

public class Warnings : MonoBehaviour,IPunObservable
{
    public bool warn;
    public bool middle;
    public GameObject L;
    public Light2D light;
    public MainBoss boss;
    public int type;
    public float intensity;

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
                intensity += 0.005f;
                light.intensity = intensity;
            }
            else
            {
                intensity -= 0.005f;
                light.intensity = intensity;
            }
            if(intensity <= 0 && middle)
            {
                light.intensity = 0f;
                intensity = 0f;
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
            else if(intensity >= 1f)
            {
                middle = true;
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(intensity);
            stream.SendNext(type);
            stream.SendNext(warn);
            stream.SendNext(middle);
        }
        else
        {
            type = (int)stream.ReceiveNext();
            intensity = (float)stream.ReceiveNext();
            warn = (bool)stream.ReceiveNext();
            middle = (bool)stream.ReceiveNext();
        }
    }
}
