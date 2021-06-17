using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DarkBullet : MonoBehaviourPun
{
    public string facingDirection;
    public int damage;
    public Rigidbody2D rb2d;
    public float attackSpeed;
    public Transform castPos;
    public Transform castPos1;
    public Transform castPos2;
    public float baseCastDist;
    public Vector3 shootdir;

    // Start is called before the first frame updateZ
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        transform.eulerAngles = new Vector3(0,0,GetAngleFromVectorFloat(shootdir));
        if (facingDirection != "right")
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(IsHittingWall())
        {
            Destroy();
        }
        else
        {
            Rush();
        }
    }
    public void Rush()
    {
        float vX = -attackSpeed;
        if (facingDirection == "right")
        {
            vX = attackSpeed;
        }
        transform.position += shootdir * Time.deltaTime * attackSpeed;
    }

    bool IsHittingWall()
    {
        bool val = false;
        float castDist = baseCastDist;
        //define the cast distance
        if (facingDirection != "right")
        {
            castDist = -baseCastDist;
        }
        //determine the target destination based on the cast distance
        Vector3 targetPos = castPos.position;
        Vector3 targetPos1 = castPos1.position;
        Vector3 targetPos2 = castPos2.position;
        targetPos.x += castDist;
        targetPos1.x += castDist;
        targetPos2.x += castDist;
        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(castPos1.position, targetPos1, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(castPos2.position, targetPos2, 1 << LayerMask.NameToLayer("Ground")))
        {

            val = true;
        }
        else
        {
            val = false;
        }
        return val;
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(n < 0)
        {
            n += 360;
        }
        return n;
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
