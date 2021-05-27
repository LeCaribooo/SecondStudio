using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("test1");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("test2");
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }
}
