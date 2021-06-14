using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levier : MonoBehaviour
{
    public GameObject[] doors;

    private void Unlock()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].GetComponent<Animator>().enabled = true;
            
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.A) && collision.CompareTag("Player"))
        {
            Unlock();
        }
    }
}
