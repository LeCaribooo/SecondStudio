using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SceneBossSpawner : MonoBehaviourPun
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject[] bosses;

    void Start()
    {
        for (int i = 0; i < bosses.Length; i++)
        {
            if(PhotonNetwork.IsMasterClient)
                PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", bosses[i].name), spawnPoints[i].position, Quaternion.identity);
        }
    }

}
