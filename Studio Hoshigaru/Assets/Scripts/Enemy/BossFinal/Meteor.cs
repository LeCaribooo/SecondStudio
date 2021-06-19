using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Meteor : MonoBehaviourPun
{
    public int damage;
    public float movementSpeed;
    public float alive;
    Rigidbody2D rb2d;
    public float wait;
    public float WaitMax;
    public bool waiting;
    // Start is called before the first frame update

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        wait = WaitMax;
    }

    // Update is called once per frame
    void Update()
    {
        Death();
        rb2d.velocity = new Vector2(rb2d.velocity.x, -movementSpeed);
        if(waiting)
        {
            Wait();
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

    public void Death()
    {
        alive -= Time.deltaTime;
        if(alive <= 0)
        {
            Destroy();
        }
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

    [PunRPC]

    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }
}
