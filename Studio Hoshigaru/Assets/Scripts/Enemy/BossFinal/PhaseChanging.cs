using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhaseChanging : MonoBehaviourPun, IPunObservable
{
    public GameObject P1;
    public GameObject P2;
    public GameObject P3;

    public bool p1;
    public bool p2;
    public bool changed;
    public bool ended;

    

    // Start is called before the first frame update

    void Update()
    {
        if(p2 && !changed)
        {
            base.photonView.RPC("P1A", RpcTarget.All, false);
            base.photonView.RPC("P2A", RpcTarget.All, true);
            changed = true;
            p2 = false;
        }
        if(p1 && changed)
        {
            base.photonView.RPC("P2A", RpcTarget.All, false);
            base.photonView.RPC("P1A", RpcTarget.All, true);
            changed = true;
            p1 = false;
        }
        if(ended && changed && P3 != null)
        {
            base.photonView.RPC("P2A", RpcTarget.All, false);
            base.photonView.RPC("P3A", RpcTarget.All, true);
            P2 = P3;
            changed = false;
        }
    }

    [PunRPC]
    public void P2A(bool active)
    {
        P2.SetActive(active);
    }
    [PunRPC]
    public void P1A(bool active)
    {
        P1.SetActive(active);
    }
    [PunRPC]
    public void P3A(bool active)
    {
        P3.SetActive(active);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(p1);
            stream.SendNext(p2);
            stream.SendNext(changed);
            stream.SendNext(ended);
        }
        else
        {
            p1 = (bool)stream.ReceiveNext();
            p2 = (bool)stream.ReceiveNext();
            changed = (bool)stream.ReceiveNext();
            ended = (bool)stream.ReceiveNext();
        }
    }
}
