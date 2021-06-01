using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WarriorAttack : MonoBehaviourPun
{
    // Start is called before the first frame update
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GetComponentInParent<WarriorBoss>().alreadytouched)
            {
                GameObject player = collision.gameObject;
                player.GetComponent<Health>().numOfHits -= damage;
                GetComponentInParent<WarriorBoss>().alreadytouched = false;
            }
        }
    }
}
