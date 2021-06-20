using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Photon.Pun;

public class HitboxDamage : MonoBehaviourPun, IPunObservable
{

    public PlayerControler playerControler;


    public int dmg;
    public float knockbackStrength;
    public WeaponSelection weaponSelection;
    public PhotonView PV;

    int dmgDealt;
    float knockBackDealt;

    private void Start()
    {
        playerControler = GetComponentInParent<PlayerControler>();
        dmgDealt = dmg + playerControler.playerForce;
        knockBackDealt = knockbackStrength + playerControler.playerKnockback;
    }


    private void Update()
    {
        dmgDealt = dmg + playerControler.playerForce;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActiveAndEnabled)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                GameObject enemy = other.gameObject;
                other.gameObject.GetComponentInParent<EnemyHealth>().health -= dmgDealt;
                if (other.gameObject.name == "shinigami(Clone)"|| other.gameObject.name == "shinigamiLight(Clone)")
                {
                    other.gameObject.GetComponent<AIPath>().enabled = false;
                    Knockback(enemy, other.gameObject.GetComponent<Rigidbody2D>());
                    StartCoroutine(Wait(other));
                }
                else
                    Knockback(enemy, other.gameObject.GetComponent<Rigidbody2D>());
            }
            else if (other.gameObject.CompareTag("Boss"))
            {
                GameObject enemy = other.gameObject;
                other.gameObject.GetComponentInParent<EnemyHealth>().health -= dmgDealt;
            }
            
        }
        
    }

    IEnumerator Wait(Collider2D other)
    {
        yield return new WaitForSeconds(.25f);
        other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        other.gameObject.GetComponent<AIPath>().enabled = true;
    }

    private void Knockback(GameObject enemy, Rigidbody2D rdb2)
    {
        string weapon = weaponSelection.actualWeaponString;
        switch (weapon)
        {
            case "hasSword":
            case "hasHammer":
            case "hasShuriken":
                Vector2 direction = rdb2.transform.position - transform.position;
                direction.y = 0;
                rdb2.AddForce(direction.normalized * knockBackDealt, ForceMode2D.Impulse);
                break;
            case "hasBow":
                break;
            default:
                break;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(knockBackDealt);
            stream.SendNext(dmgDealt);
        }
        else
        {
            knockBackDealt = (float)stream.ReceiveNext();
            dmgDealt = (int)stream.ReceiveNext();
        }
    }
}
