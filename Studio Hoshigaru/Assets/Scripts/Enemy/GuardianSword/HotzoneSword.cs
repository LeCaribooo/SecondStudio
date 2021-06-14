using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotzoneSword : MonoBehaviour
{
    private GuardianSword enemyParent;
    private bool inRange;
    private Animator anim;

    private void Awake()
    {
        enemyParent = GetComponentInParent<GuardianSword>();
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.triggerArea.SetActive(true);
            enemyParent.target = null;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
                anim.SetBool("canWalk", false);
            }
        }
    }
}
