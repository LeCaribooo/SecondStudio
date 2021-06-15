using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DashAnimation : MonoBehaviourPun
{
    public void Destruc()
    {
        base.photonView.RPC("DestroyOnline", RpcTarget.All);
    }

    [PunRPC]
    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }
}
