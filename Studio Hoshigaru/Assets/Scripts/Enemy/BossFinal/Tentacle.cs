using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tentacle : MonoBehaviourPun, IPunObservable
{
    public int damage;
    public float alive;
    public Animator anim;
    public float wait;
    public float WaitMax;
    public bool waiting;

    private void Start()
    {
        wait = WaitMax;
    }
    void Update()
    {
        Life();
        if(waiting)
        {
            Wait();
        }
    }

    public void Wait()
    {
        wait -= Time.deltaTime;
        if(wait <= 0)
        {
            wait = WaitMax;
            waiting = false;
        }
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
        if (collision.gameObject.CompareTag("Player") && !waiting)
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Health>().numOfHits -= damage;
            waiting = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !waiting)
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Health>().numOfHits -= damage;
            waiting = true;
        }
    }



    public void Destroy()
    {
        base.photonView.RPC("DestroyOnline", RpcTarget.All);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(wait);
        }
        else
        {
            wait = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]

    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }

}
