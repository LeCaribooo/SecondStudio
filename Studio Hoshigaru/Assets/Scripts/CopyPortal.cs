using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CopyPortal : MonoBehaviourPun
{
    public bool ComeBack;
    
    static bool created = false;

    public string portalName;

    private void Awake()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            if (!created)
            {
                created = true;
            }
            else
            {
                GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
                for (int i = 0; i < portals.Length; i++)
                {
                    CopyPortal p = portals[i].GetComponent<CopyPortal>();
                    if (portalName == p.portalName)
                    {
                        ComeBack = p.ComeBack;
                        PhotonNetwork.Destroy(p.gameObject);
                    }
                    break;
                }

            }
        }
    }

}
