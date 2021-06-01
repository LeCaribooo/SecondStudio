using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WarriorAttack : MonoBehaviourPun
{
    // Start is called before the first frame update
    public int damage;
    private Health health = null;
    public PhotonView PV;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GetComponentInParent<WarriorBoss>().alreadytouched)
            {
                GameObject player = collision.gameObject;
                health = player.GetComponent<Health>();
                PV.RPC("Dommage", RpcTarget.All);
                GetComponentInParent<WarriorBoss>().alreadytouched = false;
            }
        }
    }

    [PunRPC]
    public void Dommage()
    {
        health.numOfHits -= damage;
    }
}
