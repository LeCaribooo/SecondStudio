using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tentacle : MonoBehaviourPun
{
    public int damage;
    public float alive;
    public Animator anim;
 
    void Update()
    {
        Life();
    }

    public void Life()
    {
        alive -= Time.deltaTime;
        if(alive <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        anim.SetBool("Dead", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Health>().numOfHits -= damage;
        }
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
