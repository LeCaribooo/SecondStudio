using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackSwordBoss : MonoBehaviourPun
{
    private Health health;
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            health = player.GetComponent<Health>();
            base.photonView.RPC("Damage",RpcTarget.All);
        }
    }

    [PunRPC]
    public void Damage()
    {
        health.numOfHits -= damage;
    }
}
