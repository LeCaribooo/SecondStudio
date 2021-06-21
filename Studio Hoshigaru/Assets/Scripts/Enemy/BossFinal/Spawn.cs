using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Spawn : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "Boss"), transform.position, Quaternion.identity);
        }
    }
}
