using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstocDamage : MonoBehaviour
{
    public int dmg;

    private void Start()
    {
        
    }

    private void Update()
    {
        Debug.Log("EstocDamage" + isActiveAndEnabled);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Enemy") && isActiveAndEnabled)
        {
            collision.gameObject.GetComponentInParent<EnemyHealth>().health -= dmg;
        }
    }
}
