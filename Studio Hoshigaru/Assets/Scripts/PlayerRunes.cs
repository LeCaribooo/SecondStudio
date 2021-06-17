using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRunes : MonoBehaviourPun, IPunObservable
{
    public int nbOfRunes;

    public int parts;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(nbOfRunes);
            stream.SendNext(parts);
        }
        else
        {
            nbOfRunes = (int)stream.ReceiveNext();
            parts = (int)stream.ReceiveNext();
        }
    }
}
