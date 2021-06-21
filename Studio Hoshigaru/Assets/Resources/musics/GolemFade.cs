using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemFade : MonoBehaviour
{
    public AudioClip[] musics;
    private float volume;
    private bool check = false;
    public Golem boss;
    public bool end;
    // Start is called before the first frame update
    void Start()
    {
        volume = 1f;
        GetComponent<AudioSource>().volume = 1f;
        GetComponent<AudioSource>().clip = musics[0];
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(boss == null)
        {
            boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Golem>();
        }
        if (boss.deade && !check)
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
                GetComponent<AudioSource>().Play();
                check = true;
            }
        }
        else if(check && !end)
        {
            if (GetComponent<AudioSource>().volume < 1)
            {
                volume += 0.001f;
                GetComponent<AudioSource>().volume = volume;
            }
            else
            {
                end = true;
            }
        }
    }
}
