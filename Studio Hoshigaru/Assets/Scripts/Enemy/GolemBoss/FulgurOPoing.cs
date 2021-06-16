using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FulgurOPoing : MonoBehaviourPun
{
    public float attacktimer;
    public string facingDirection = "right";
    public int damage;
    public Rigidbody2D rb2d;
    public float placementSpeed;
    public float attackSpeed;
    public GameObject boss;
    public Golem golem;
    public Transform target;
    public float y;
    private bool move;
    private bool placed;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        GameObject[] tempo = new GameObject[] { };
        tempo = GameObject.FindGameObjectsWithTag("Boss");
        boss = tempo[0];
        golem = boss.GetComponent<Golem>();
        target = golem.target;
        y = target.position.y + 0.1f;
        move = true;
        placed = false;
        facingDirection = "right";
        string facing = golem.facingdirection;
        if(facingDirection != facing)
        {
            facingDirection = "left";
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void Update()
    {
        if(move)
        {
            Placing();
        }
        else if(placed)
        {
            Rush();
            AttackTime();
        }
    }

    public void Placing()
    {
        if(y == transform.position.y)
        {
            move = false;
            placed = true;
        }
        else
        {
            Vector2 targetPosition = new Vector2(transform.position.x, y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, placementSpeed * Time.deltaTime);
        }
    }

    public void Rush()
    {
        float vX = attackSpeed;
        if (facingDirection == "left")
        {
            vX = -attackSpeed;
        }
        rb2d.velocity = new Vector2(vX, rb2d.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Health>().numOfHits -= damage;
        }
    }

    public void AttackTime()
    {
        attacktimer -= Time.deltaTime;
        if(attacktimer <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        golem.ThrowEnd();
        base.photonView.RPC("DestroyOnline", RpcTarget.All);
    }

    [PunRPC]

    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }
}
