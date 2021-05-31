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
        Debug.Log("Ah");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Bh");
            GameObject player = collision.gameObject;
            health = player.GetComponent<Health>();
            Debug.Log(health.numOfHits);
            base.photonView.RPC("Dommage",RpcTarget.All);
        }
    }

    [PunRPC]
    public void Dommage()
    {
        Debug.Log("Ch");
        health.numOfHits -= damage;
    }
}
