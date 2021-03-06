using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CopyPortal : MonoBehaviourPun 
{
    public bool ComeBack;
    
    static bool created = false;

    public string portalName = "Default";


    private void Awake()
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
                
                if (portalName != p.portalName)
                {
                    portalName = p.portalName;
                    ComeBack = p.ComeBack;
                    Destroy(p.gameObject);
                    break;
                }
                
            }

        }

    }

}
