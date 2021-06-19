using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesAnim : MonoBehaviour
{
    public Animator anim;
    public MainBoss boss;
    public int phase;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void EndLaser()
    {
        anim.SetBool("Laser", false);
        if (phase == 1)
        {
            boss.MovingHead1();
        }
    }

    public void EndMeteor()
    {
        anim.SetBool("Meteor", false);
        if (phase == 1)
        {
            boss.MeteorWarn1();
        }
    }

    public void EndTentacle()
    {
        anim.SetBool("Tentacle", false);
        if(phase == 1)
        {
            boss.TentacleWarn1();
        }
    }
}
