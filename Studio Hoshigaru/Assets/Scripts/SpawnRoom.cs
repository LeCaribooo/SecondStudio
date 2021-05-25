using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnRoom : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawnpoint = new GameObject[4];


    // Start is called before the first frame update
    //S'occupe de faire spawn tout le monde à un point différent.
    void Start()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach (var joueur in player)
        {
            if (joueur.GetPhotonView().IsMine)
            {
                int numspawn = (joueur.GetPhotonView().ViewID / 1000) % 4;
                Debug.LogWarning("Num Spaw : " + numspawn);
                GameObject spawn = spawnpoint[numspawn];
                joueur.transform.position = spawn.transform.position;
            }
        }
        
    }



}
