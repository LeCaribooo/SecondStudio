using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HitboxDamage : MonoBehaviour
{
    public int dmg;
    public float knockbackStrength;
    public WeaponSelection weaponSelection;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject enemy = other.gameObject;
            EnemyHealth enemyhealth = other.gameObject.GetComponentInParent<EnemyHealth>();
            enemyhealth.health -= dmg;
            if(other.gameObject.name == "patroller(Clone)")
                Knockback(enemy, other.gameObject.GetComponent<Rigidbody2D>());
            else
            {
                other.gameObject.GetComponent<AIPath>().enabled = false;
                Knockback(enemy, other.gameObject.GetComponent<Rigidbody2D>());
                StartCoroutine(Wait(other));
            }
        }
        else if(other.gameObject.CompareTag("Boss"))
        {
            GameObject enemy = other.gameObject;
            EnemyHealth enemyhealth = other.gameObject.GetComponentInParent<EnemyHealth>();
            enemyhealth.health -= dmg;
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
}
