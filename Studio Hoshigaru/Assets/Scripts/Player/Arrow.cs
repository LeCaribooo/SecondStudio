using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arrow : MonoBehaviourPun
{
    Rigidbody2D rb;
    bool hasHit;
    public int dmg;
    public PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<CapsuleCollider2D>());
        }
        else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            Debug.Log("On a touché un enemi");
            collision.gameObject.GetComponentInParent<EnemyHealth>().health -= dmg;
            if (PV.IsMine)
            {
                PV.RPC("DestroyOnline", RpcTarget.All);
            }
        }
        else if (collision.CompareTag("Trigger")) { }
        else
        {
            if (PV.IsMine)
            {
                PV.RPC("DestroyOnline", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }
}
