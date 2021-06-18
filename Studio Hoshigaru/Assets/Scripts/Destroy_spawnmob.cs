using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Destroy_spawnmob : MonoBehaviour
{
    static bool created = false;

    private void Awake()
    { 
        if (!created)
        {
            created = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
