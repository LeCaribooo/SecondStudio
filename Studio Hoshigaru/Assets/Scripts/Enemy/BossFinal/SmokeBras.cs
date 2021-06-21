using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SmokeBras : MonoBehaviourPun
{
    // Start is called before the first frame update
    public GameObject smoke;
    public GameObject Growed;
    public void Grow()
    {
        base.photonView.RPC("SmokeA", RpcTarget.All, false);
        if (Growed != null)
        {
            base.photonView.RPC("GrowedA", RpcTarget.All, false);
        }
    }

    public void End()
    {
        base.photonView.RPC("SmokeA", RpcTarget.All, false);
    }

    [PunRPC]
    public void SmokeA(bool active)
    {
        smoke.SetActive(active);
    }
    [PunRPC]
    public void GrowedA(bool active)
    {
        Growed.SetActive(active);
    }
}
