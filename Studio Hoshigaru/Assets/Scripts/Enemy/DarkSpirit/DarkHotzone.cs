using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkHotzone : MonoBehaviour
{
    private DarkSpirit enemyParent;
    private Animator anim;

    private void Awake()
    {
        enemyParent = GetComponentInParent<DarkSpirit>();
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.TriggerArea.SetActive(true);
            enemyParent.target = null;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                anim.SetBool("canWalk", false);
            }
        }
    }
}
