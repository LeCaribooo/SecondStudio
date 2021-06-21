﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class launchmusic : MonoBehaviour
{
    public AudioClip[] musics;
    private float volume;
    private bool check = false;
    public MainBoss boss;
    public bool end;

    // Start is called before the first frame update
    void Start()
    {
        volume = 1f;
        boss = GameObject.FindGameObjectWithTag("BossF").GetComponent<MainBoss>();
        GetComponent<AudioSource>().volume = volume;
        GetComponent<AudioSource>().clip = musics[0];
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (boss.music1Stop)
        {
            if (GetComponent<AudioSource>().volume > 0.00)
            {
                volume -= 0.0005f;
                GetComponent<AudioSource>().volume = volume;

            }
            else
            {
                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().clip = musics[1];
                check = false;
            }
            if(boss.music2Begin)
            {
                GetComponent<AudioSource>().Play();
                boss.music1Stop = false;
            }
        }
        else if(!boss.music2End)
        {
            if (!check)
            {
                if (GetComponent<AudioSource>().volume < 1)
                {
                    volume += 0.001f;
                    GetComponent<AudioSource>().volume = volume;
                }
                else
                {
                    check = true;
                }
            }
        }
        else if(!end)
        {
            if (GetComponent<AudioSource>().volume > 0.00)
            {
                volume -= 0.01f;
                GetComponent<AudioSource>().volume = volume;
            }
            else
            {
                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().clip = musics[2];
                end = true;
            }
        }
        else
        {
            if (GetComponent<AudioSource>().volume < 1)
            {
                volume += 0.001f;
                GetComponent<AudioSource>().volume = volume;
            }
        }

    }
}
