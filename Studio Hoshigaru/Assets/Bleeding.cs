using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleeding : MonoBehaviour
{
    [SerializeField] Collider2D Collider2D;
    [SerializeField] int dotDmgs;
    [SerializeField] int nbOfDots;
    [SerializeField] float timeBetweenDots;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Enemy"|| other.gameObject.tag == "Boss") && !other.GetComponent<EnemyHealth>().isBleeding)
        {
            other.GetComponent<EnemyHealth>().isBleeding = true;
            other.GetComponent<EnemyHealth>().dotDmgs = dotDmgs;
            other.GetComponent<EnemyHealth>().nbOfDots  = nbOfDots;
            other.GetComponent<EnemyHealth>().timeBetweenDots = timeBetweenDots;
        }
    }

}
