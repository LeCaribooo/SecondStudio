using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EyesAnim : MonoBehaviour, IPunObservable
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(phase);
        }
        else
        {
            phase = (int)stream.ReceiveNext();
        }
    }
}
