using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Spawn : MonoBehaviourPun
{
    void Start()
    {
        StartCoroutine(justASecond());
    }

    IEnumerator justASecond()
    {
        yield return new WaitForSeconds(2.0f);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "Boss"), transform.position, Quaternion.identity);
        }
    }
}
