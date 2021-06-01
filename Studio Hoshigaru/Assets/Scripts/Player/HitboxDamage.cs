using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Photon.Pun;

public class HitboxDamage : MonoBehaviourPun
{
    public int dmg;
    public float knockbackStrength;
    public WeaponSelection weaponSelection;
    public PhotonView PV;
    private EnemyHealth enemyhealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject enemy = other.gameObject;
            enemyhealth = other.gameObject.GetComponentInParent<EnemyHealth>();
            if (PV.IsMine)
            {
                PV.RPC("Dommage", RpcTarget.All);
            }
            if (other.gameObject.name == "shinigami(Clone)")
            {
                other.gameObject.GetComponent<AIPath>().enabled = false;
                Knockback(enemy, other.gameObject.GetComponent<Rigidbody2D>());
                StartCoroutine(Wait(other));
            }
            else
                Knockback(enemy, other.gameObject.GetComponent<Rigidbody2D>());
        }
        else if(other.gameObject.CompareTag("Boss"))
        {
            GameObject enemy = other.gameObject;
            enemyhealth = other.gameObject.GetComponentInParent<EnemyHealth>();
            if(PV.IsMine)
                PV.RPC("Dommage", RpcTarget.All);
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
                rdb2.AddForce(direction.normalized * knockbackStrength, ForceMode2D.Impulse);
                break;
            case "hasBow":
                break;
            default:
                break;
        }
    }

    [PunRPC]
    public void Dommage()
    {
        enemyhealth.health -= dmg;
    }
}
