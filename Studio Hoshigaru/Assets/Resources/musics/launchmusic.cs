using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class launchmusic : MonoBehaviour
{
    public AudioClip[] musics;
    private float volume;
    private int musiccount = 0;
    private bool check = false;
    private bool changemusic = false;



    // Start is called before the first frame update
    void Start()
    {
        volume = 0f;
        GetComponent<AudioSource>().volume = volume;
        GetComponent<AudioSource>().clip = musics[musiccount];
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (changemusic)
        {
            if (GetComponent<AudioSource>().volume > 0.05)
            {
                volume -= 0.01f;
                GetComponent<AudioSource>().volume = volume;

            }
            else
            {
                GetComponent<AudioSource>().Stop();
                musiccount = (musiccount + 1) % 2;
                GetComponent<AudioSource>().clip = musics[musiccount];
                GetComponent<AudioSource>().Play();
                changemusic = false;
                check = false;
            }
        }
        else
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
        
    }

    /*private void OnTriggerExit2D()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView PV = player.GetComponent<PhotonView>();
            if (PV.IsMine)
            {
                if (collider2D.tag == "Player")
                {
                    check = false;
                    break;
                }
            }
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (collider2D.tag == "Player")
        {
            foreach (GameObject player in players)
            {
                if (player.GetPhotonView().IsMine)
                {
                    changemusic = true;
                    break;
                }
            }
        }
            
            
                
    }

    

}
