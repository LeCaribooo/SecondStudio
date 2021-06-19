using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSmoke : MonoBehaviour
{
    public int Damage;
    public float wait;
    public float WaitMax;
    public bool waiting;

    private void Start()
    {
        wait = WaitMax;
    }
    private void Update()
    {
        if(waiting)
        {
            Wait();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !waiting)
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Health>().numOfHits -= Damage;
            waiting = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !waiting)
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Health>().numOfHits -= Damage;
            waiting = true;
        }
    }

    public void Wait()
    {
        wait -= Time.deltaTime;
        if (wait <= 0)
        {
            wait = WaitMax;
            waiting = false;
        }
    }
}
