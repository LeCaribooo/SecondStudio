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
        boss.MovingHead();
    }

    public void EndMeteor()
    {
        anim.SetBool("Meteor", false);
        boss.MeteorWarn();
    }

    public void EndTentacle()
    {
        anim.SetBool("Tentacle", false);
        boss.TentacleWarn();
    }
}
