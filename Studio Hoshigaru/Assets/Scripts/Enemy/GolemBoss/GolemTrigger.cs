using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemTrigger : MonoBehaviour
{
    public Golem script;
    private void Start()
    {
        script = GetComponentInParent<Golem>();
    }
    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            script.anim.SetBool("Block0", false);
            script.Triggered = true;
        }
    }
}
