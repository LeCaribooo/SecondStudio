using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class launchmusic : MonoBehaviour
{
    private float volume;
    private bool check = false;
    

    
    // Start is called before the first frame update
    void Start()
    {
        volume = 0f;
        GetComponent<AudioSource>().volume = volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (check)
        {
            if (GetComponent<AudioSource>().volume > 0.5)
            {
                volume += 0.01f;
            }
            else
            {
                volume += 0.001f;
            }
            GetComponent<AudioSource>().volume = volume;
            
        }
        else
        {
            if (GetComponent<AudioSource>().volume > 0f)
            {
                GetComponent<AudioSource>().volume = volume;
                volume -= 0.005f;
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collider2D)
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
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView PV = player.GetComponent<PhotonView>();
            if (PV.IsMine)
            {
                if (collider2D.tag == "Player")
                {
                    check = true;
                    GetComponent<AudioSource>().Play();
                }
            }
        }
                
    }

    

}
