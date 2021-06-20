using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Laser : MonoBehaviourPun
{
    
    public string facingDirection;
    public MainBoss boss;

    private void Start()
    {
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
}
