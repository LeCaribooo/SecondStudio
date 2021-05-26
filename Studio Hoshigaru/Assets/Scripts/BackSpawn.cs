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
            Debug.LogWarning("Nom du portail : " + p.nameportal);
            Debug.LogWarning("Nom du CopyPortal : " + p.CopyPortal.portalName);
            if (p.nameportal == p.CopyPortal.portalName)
            {
                if (p.CopyPortal.ComeBack)
                {
                    GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
                    foreach (var joueur in player)
                    {
                        if (joueur.GetPhotonView().IsMine)
                        {
                            int numspawn = (joueur.GetPhotonView().ViewID / 1000) % 4;
                            Debug.LogWarning("Num Spaw : " + numspawn);
                            GameObject spawn = p.spawnpoint[numspawn];
                            joueur.transform.position = spawn.transform.position;
                        }
                    }

                    Debug.Log("Je remet CP à False");
                    p.CopyPortal.ComeBack = false;
                    break;
                }
            }
        }
        
    }


}
