using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SceneBossSpawner : MonoBehaviourPun
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject[] bosses;
    public bool spawned;
    void Start()
    {
        StartCoroutine(justASecond());
    }

    IEnumerator justASecond()
    {
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < bosses.Length; i++)
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", bosses[i].name), spawnPoints[i].position, Quaternion.identity);
        }
        spawned = true;
    }

}
