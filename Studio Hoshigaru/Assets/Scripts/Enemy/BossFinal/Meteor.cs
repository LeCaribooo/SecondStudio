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
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Death();
        rb2d.velocity = new Vector2(rb2d.velocity.y, -movementSpeed);
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
