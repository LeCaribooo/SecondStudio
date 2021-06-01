using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackPatroller : MonoBehaviourPun
{
    public int damage;
    private Health health = null;
    public PhotonView PV;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !GetComponentInParent<Patroller>().alreadyAttacked)
        {
            GameObject player = collision.gameObject;
            health = player.GetComponent<Health>();
            if(PV.IsMine)
                PV.RPC("Dommage", RpcTarget.All);
            GetComponentInParent<Patroller>().alreadyAttacked = true;
        }
    }

    [PunRPC]
    public void Dommage()
    {
        health.numOfHits -= damage;
    }
}
