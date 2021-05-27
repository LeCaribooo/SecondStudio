using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBegin : MonoBehaviour
{
    // Start is called before the first frame update
    public WarriorBoss script;
    private void Start()
    {
        script = GetComponentInParent<WarriorBoss>();
    }
    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            script.started = true;
        }
    }
}
