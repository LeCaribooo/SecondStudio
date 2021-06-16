using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShockWave : MonoBehaviourPun
{

    public int damage;

    public GameObject boss;
    public string facingDirection;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] tempo = new GameObject[] { };
        tempo = GameObject.FindGameObjectsWithTag("Boss");
        boss = tempo[0];
        string facing = boss.GetComponent<Golem>().facingdirection;
        if (facing != facingDirection)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Update is called once per frame

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
        Golem golem = boss.GetComponent<Golem>();
        golem.EndAttack();
        base.photonView.RPC("DestroyOnline", RpcTarget.All);
    }

    [PunRPC]

    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }
}
