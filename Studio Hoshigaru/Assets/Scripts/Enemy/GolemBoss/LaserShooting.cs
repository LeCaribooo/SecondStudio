using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LaserShooting : MonoBehaviourPun, IPunObservable
{
    // Start is called before the first frame update
    public int damage;

    public GameObject boss;
    public string facingDirection;

    private void Start()
    {
        facingDirection = "right";
        GameObject[] tempo = new GameObject[] { };
        tempo = GameObject.FindGameObjectsWithTag("Boss");
        boss = tempo[0];
        string facing = boss.GetComponent<Golem>().facingdirection;
        if(facing != facingDirection)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        transform.position = boss.GetComponent<Golem>().LaserPos.position;
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
        Golem golem = boss.GetComponent<Golem>();
        golem.LaserStop();
        base.photonView.RPC("DestroyOnline", RpcTarget.All);
    }


    [PunRPC]

    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(damage);
            stream.SendNext(facingDirection);
        }
        else
        {
            damage = (int)stream.ReceiveNext();
            facingDirection = (string)stream.ReceiveNext();
        }
    }
}
