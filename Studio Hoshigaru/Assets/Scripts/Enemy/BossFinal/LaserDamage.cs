using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public int damage;
    public float wait;
    public float WaitMax;
    public bool waiting;
    // Start is called before the first frame update
    void Start()
    {
        wait = WaitMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            Wait();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !waiting)
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Health>().numOfHits -= damage;
            waiting = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !waiting)
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Health>().numOfHits -= damage;
            waiting = true;
        }
    }
}
