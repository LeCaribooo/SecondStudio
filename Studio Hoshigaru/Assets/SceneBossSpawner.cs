using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SceneBossSpawner : MonoBehaviourPun
{
    public Transform[] spawnPoints;
    public GameObject[] bosses;

    private void Start()
    {
        for (int i = 0; i < bosses.Length; i++)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", bosses[i].name), spawnPoints[i].position, Quaternion.identity);
        }
    }

}
