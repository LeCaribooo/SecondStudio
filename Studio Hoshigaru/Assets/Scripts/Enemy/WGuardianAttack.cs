using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WGuardianAttack : MonoBehaviourPun
{
    // Start is called before the first frame update
    public int damage;
    public PhotonView PV;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GetComponentInParent<GuardianWarrior>().alreadytouched)
            {
                GameObject player = collision.gameObject;
                player.GetComponent<Health>().numOfHits -= damage;
                GetComponentInParent<GuardianWarrior>().alreadytouched = false;
            }
        }
    }
}
