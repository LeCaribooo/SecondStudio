using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Laser : MonoBehaviourPun
{
    public int damage;
    public string facingDirection;
    public MainBoss boss;
    public float wait;
    public float WaitMax;
    public bool waiting;

    private void Start()
    {
        wait = WaitMax;
        if (facingDirection == "left")
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    private void Update()
    {
        if(waiting)
        {
            Wait();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&& !waiting)
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
    public void Wait()
    {
        wait -= Time.deltaTime;
        if (wait <= 0)
        {
            wait = WaitMax;
            waiting = false;
        }
    }

    public void EndLaser()
    {
        boss.facingDirection = facingDirection;
        boss.movingFromLaser = true;
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
