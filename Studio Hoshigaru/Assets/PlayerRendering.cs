using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerRendering : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerDisplay;
    Player[] players;
 
    // Start is called before the first frame update
    void Start()
    {
        players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].IsLocal)
            {
                GameObject player = Instantiate(playerDisplay, playerListContent);
                player.GetComponent<PlayerDisplay>().SetUp(players[i], i);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
