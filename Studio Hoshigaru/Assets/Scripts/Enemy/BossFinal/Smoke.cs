using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Smoke : MonoBehaviourPun
{
    public GameObject smoke;
    public void Grow()
    {
        transform.localScale = new Vector3(25, 25, 1);
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
}
