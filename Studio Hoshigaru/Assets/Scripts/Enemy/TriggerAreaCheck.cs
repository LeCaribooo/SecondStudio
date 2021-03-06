using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaCheck : MonoBehaviour
{
    private Patroller enemyParent;

    private void Awake()
    {
        enemyParent = GetComponentInParent<Patroller>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.target = collider.transform;
            enemyParent.inRange = true;
            enemyParent.hotZone.SetActive(true);
            if (enemyParent.source != null)
            {
                enemyParent.source.Play();
            }
        }
    }
}
