using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WaitingForSpawn : MonoBehaviour
{
    [SerializeField]Behaviour pr;
    GameObject[] playersInScene;
    Player[] playerOnline;

    void Start()
    {
        playersInScene = GameObject.FindGameObjectsWithTag("Player");
        playerOnline = PhotonNetwork.PlayerList;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerOnline.Length == playersInScene.Length)
        {
            pr.enabled = true;
            this.enabled = false;
        }
        else
        {
            playersInScene = GameObject.FindGameObjectsWithTag("Player");
        }
    }
}
