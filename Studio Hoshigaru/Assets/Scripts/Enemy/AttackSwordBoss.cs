using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackSwordBoss : MonoBehaviourPun
{
    private Health health = null;
    public int damage;
    public PhotonView PV;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            health = player.GetComponent<Health>();
            PV.RPC("Dommage",RpcTarget.All);
        }
    }

    [PunRPC]
    public void Dommage()
    {
        health.numOfHits -= damage;
    }
}
