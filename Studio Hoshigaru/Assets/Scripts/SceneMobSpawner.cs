using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SceneMobSpawner : MonoBehaviourPun
{
    [SerializeField] Transform[] positions;
    [SerializeField] GameObject mob;

    void Start()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", mob.name), positions[i].position, Quaternion.identity);
        }
    }
}
