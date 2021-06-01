using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackPatroller : MonoBehaviourPun
{
    public int damage;
    public PhotonView PV;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !GetComponentInParent<Patroller>().alreadyAttacked)
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Health>().numOfHits -= damage;
            GetComponentInParent<Patroller>().alreadyAttacked = true;
        }
    }
}
