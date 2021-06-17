using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPhase : MonoBehaviourPun
{
    [SerializeField]
    private GameObject gestionspawn;

    [SerializeField]
    private GameObject[] spawnpoint = new GameObject[4];

    private static bool created = false;


    private void Awake()
    {
        if (!created)
        {
            created = true;
        }
        else
        {
            Destroy(this);
            Destroy(gestionspawn);
        }
    }

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
                GameObject spawn = spawnpoint[numspawn];
                joueur.transform.position = spawn.transform.position;
            }
        }

        Destroy(this);
        Destroy(gestionspawn);

    }

}
