using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSSword : MonoBehaviour
{
    private GuardianSword enemyParent;

    private void Awake()
    {
        enemyParent = GetComponentInParent<GuardianSword>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.target = collider.transform;
            enemyParent.hotZone.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.target = collider.transform;
            enemyParent.hotZone.SetActive(true);
        }
    }
}
