using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneWarrior : MonoBehaviour
{
    // Start is called before the first frame update
    private GuardianWarrior enemyParent;
    private bool inRange;
    private Animator anim;

    private void Awake()
    {
        enemyParent = GetComponentInParent<GuardianWarrior>();
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.triggerArea.SetActive(true);
            enemyParent.target = null;
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
                anim.SetBool("walk", false);
            }
        }
    }
}
