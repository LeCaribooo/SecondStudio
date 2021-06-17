using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadHotzone : MonoBehaviour
{
    private BadJuju enemyParent;
    private Animator anim;

    private void Awake()
    {
        enemyParent = GetComponentInParent<BadJuju>();
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.TriggerArea.SetActive(true);
            enemyParent.target = null;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
                anim.SetBool("canWalk", false);
            }
        }
    }
}
