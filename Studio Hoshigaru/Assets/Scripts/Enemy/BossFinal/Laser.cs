using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Laser : MonoBehaviourPun,IPunObservable
{
    
    public string facingDirection;
    public MainBoss boss;

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("BossF").GetComponent<MainBoss>();
        if (facingDirection == "left")
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }


    public void EndLaser()
    {
        boss.facingDirection = facingDirection;
        boss.movingFromLaser = true;
    }

    public void Destroy()
    {
        base.photonView.RPC("DestroyOnline", RpcTarget.All);
    }


    [PunRPC]

    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(facingDirection);
        }
        else
        {
            facingDirection = (string)stream.ReceiveNext();
        }
    }
}
