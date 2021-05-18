using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HitboxDamage : MonoBehaviour
{
    public GameObject player;
    public int dmg;
    public WeaponSelection weaponSelection;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject enemy = other.gameObject;
            EnemyHealth enemyhealth = other.gameObject.GetComponentInParent<EnemyHealth>();
            Debug.Log("etsfdq");
            enemyhealth.health -= dmg;
            bool right = player.GetComponent<PlayerControler>().facingRight;
            Debug.Log(other.gameObject.GetComponent<Rigidbody2D>().velocity);
            Knockback(enemy, other.gameObject.GetComponent<Rigidbody2D>().velocity, right);
        }
    }


    private void Knockback(GameObject enemy, Vector2 rdb2, bool right)
    {
        string weapon = weaponSelection.actualWeaponString;
        switch (weapon)
        {
            case "hasSword":
                Debug.Log("it's ok");
                if(rdb2 == Vector2.zero)
                {
                    rdb2 = new Vector2(40, 40);
                }
                else if (!right)
                {
                    rdb2 = transform.right * 40;
                }
                else
                {
                    rdb2 = -transform.right * 40;
                }
                break;
            case "hasShuriken":
            case "hasBow":
                Debug.Log("it's ok");
                break;
            case "hasHammer":
                Debug.Log("it's ok");
                if (!right)
                {
                    rdb2= Vector2.right * 6;
                }
                else
                {
                    rdb2 = Vector2.left * 6;
                }
                break;
        }
    }
}
