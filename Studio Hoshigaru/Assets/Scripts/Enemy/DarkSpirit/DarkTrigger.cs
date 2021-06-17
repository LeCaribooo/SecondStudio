using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkTrigger : MonoBehaviour
{
    private DarkSpirit enemyParent;

    // Start is called before the first frame update
    void Awake()
    {
        enemyParent = GetComponentInParent<DarkSpirit>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.target = collider.transform;
            enemyParent.Hotzone.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.target = collider.transform;
            enemyParent.Hotzone.SetActive(true);
        }
    }
}
