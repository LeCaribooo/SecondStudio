using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class BackSpawn : MonoBehaviourPun
{
    [SerializeField]
    private Player_portal[] portals;


    private void Start()
    {

        foreach (Player_portal p in portals)
        {
            if (p.CopyPortal.ComeBack)
            {
                GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
                int i = 0;
                foreach (var joueur in player)
                {
                    if (joueur.GetPhotonView().IsMine)
                    {
                        GameObject spawn = p.spawnpoint[i];
                        joueur.transform.position = spawn.transform.position;
                        i++;
                    }
                }

                Debug.Log("Je remet CP à False");
                p.CopyPortal.ComeBack = false;
                break;
            }
        }
        
    }


}
