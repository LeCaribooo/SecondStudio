using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShurikenLauncher : MonoBehaviourPun, IPunObservable
{
    bool go;

    GameObject player;
    GameObject[] players;
    Transform itemToRotate;
    Vector2 locationInFrontOfPlayer;
    private EnemyHealth enemyhealth;
    PlayerControler playerControler;
    Shuriken shuriken;
    public PhotonView PV;
    public bool facingRight = false;
    public int dmg;

    int dmgDealt;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        go = false;
        player = getPlayer();
        shuriken = player.GetComponentInChildren<Shuriken>();
        shuriken.gameObject.SetActive(false);
        itemToRotate = this.gameObject.transform;
        playerControler = player.GetComponent<PlayerControler>();
        if (!playerControler.facingRight)
        {
            locationInFrontOfPlayer = new Vector2(player.transform.position.x + 5, player.transform.position.y);
        }
        else
        {
            locationInFrontOfPlayer = new Vector2(player.transform.position.x - 5, player.transform.position.y);
        }
        dmgDealt = dmg + playerControler.playerForce;
        StartCoroutine(Boom());
    }

    GameObject getPlayer()
    {
        foreach(GameObject player in players)
        {
            if (player.GetPhotonView().Owner == PV.Owner)
                return player;
        }
        return null;
    }

    IEnumerator Boom()
    {
        go = true;
        yield return new WaitForSeconds(0.5f);
        go = false;
    }

    void Update()
    {
        if (PV.IsMine)
        {
            itemToRotate.transform.Rotate(0, 0, Time.deltaTime * 500);

            if (go)
            {
                transform.position = Vector2.MoveTowards(transform.position, locationInFrontOfPlayer, Time.deltaTime * 15);
            }
            if (!go)
            {
                if (!playerControler.facingRight)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x + 0.5f, player.transform.position.y), Time.deltaTime * 15);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x - 0.5f, player.transform.position.y), Time.deltaTime * 15);
                }
            }
        }
        if (!go && Vector2.Distance(player.transform.position, transform.position) <= 0.5f)
        {
            shuriken.numberOfShuriken++;
            base.photonView.RPC("SetGameObjectActive", RpcTarget.All, true);
            PhotonNetwork.Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject enemy = other.gameObject;
            other.gameObject.GetComponentInParent<EnemyHealth>().health -= dmgDealt;
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            GameObject enemy = other.gameObject;
            other.gameObject.GetComponentInParent<EnemyHealth>().health -= dmgDealt;
        }
    }

    [PunRPC]
    void SetGameObjectActive(bool isActive)
    {
        shuriken.gameObject.SetActive(isActive);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(dmgDealt);
        }
        else
        {
            dmgDealt = (int)stream.ReceiveNext();
        }
    }
}
