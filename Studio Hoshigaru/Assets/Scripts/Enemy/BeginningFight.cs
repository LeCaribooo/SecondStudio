using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningFight : MonoBehaviour
{
    public SwordBoss script;
    private void Start()
    {
        script = GetComponentInParent<SwordBoss>();
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
