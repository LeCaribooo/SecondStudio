using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerRendering : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerDisplay;
    Player[] playersOnline;
    GameObject[] playersInScene;

 
    // Start is called before the first frame update
    void Start()
    {
        playersInScene = GameObject.FindGameObjectsWithTag("Player");
        playersOnline = PhotonNetwork.PlayerList;
        playersInScene = SortPlayer();
        Debug.Log("Online " + playersOnline.Length + "In scene " + playersInScene.Length);
        for (int i = 0; i < playersOnline.Length; i++)
        {
            if (!playersOnline[i].IsLocal)
            {
                GameObject player = Instantiate(playerDisplay, playerListContent);
                player.GetComponent<PlayerDisplay>().SetUp(playersOnline[i], playersInScene[i]);
            }
        }
    }

    GameObject[] SortPlayer()
    {
        GameObject[] sortedPlayers = new GameObject[playersInScene.Length];
        for (int i = 0; i < playersOnline.Length; i++)
        {
            for (int j = 0; j < playersInScene.Length; j++)
            {
                if(playersInScene[j].GetPhotonView().Owner == playersOnline[i])
                {
                    sortedPlayers[i] = playersInScene[j];
                }
            }
        }
        return sortedPlayers;
    }

}
