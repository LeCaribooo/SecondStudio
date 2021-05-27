using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AICollision : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("test2");
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(),gameObject.GetComponent<Collider2D>());
        }
    }
}
