using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerRendering : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerDisplay;
    Player[] playersOnline;
    GameObject[] playersInScene;
    GameObject[] prefabs;
    Player masterClient;
    [SerializeField] Text otherPlayerName;


    // Start is called before the first frame update
    void Start()
    {
        playersInScene = GameObject.FindGameObjectsWithTag("Player");
        playersOnline = PhotonNetwork.PlayerList;
        prefabs = new GameObject[playersOnline.Length];
        playersInScene = SortPlayer();
        for (int i = 0; i < playersOnline.Length; i++)
        {
            if (playersOnline[i].IsMasterClient)
                masterClient = playersOnline[i];
            if (!playersOnline[i].IsLocal)
            { 
                playersInScene[i].GetComponentInChildren<PlayerRendering>().otherPlayerName.text = playersOnline[i].NickName;
                GameObject player = Instantiate(playerDisplay, playerListContent);
                player.GetComponent<PlayerDisplay>().SetUp(playersOnline[i], playersInScene[i]);
                prefabs[i] = player;
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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer == masterClient)
        {
            StartCoroutine(Leave());
        }
        else
        {
            for (int i = 0; i < playersOnline.Length; i++)
            {
                if (playersOnline[i] == otherPlayer)
                    Destroy(prefabs[i]);
            }
        }
    }

    IEnumerator Leave()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(0);
    }
}
