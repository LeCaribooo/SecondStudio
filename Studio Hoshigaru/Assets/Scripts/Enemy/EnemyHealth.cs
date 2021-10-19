using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyHealth : MonoBehaviour, IPunObservable
{
    public int health;
    public bool isBleeding;
    public int dotDmgs;
    public int nbOfDots;
    private int dotTaken;
    public float timeBetweenDots;
    private bool hasBleed;
    private float countdown;
    public bool die;
    public int damage;
    public bool death;
    private void Start()
    {
        hasBleed = false;
        isBleeding = false;
        dotTaken = 0;
        countdown = 5f;
        damage = 1;
    }

    private void Update()
    {

        if(die)
        {
            InstantDeath();
        }
        if(death)
        {
            Death();
        }
        if (isBleeding && !hasBleed)
        {
            StartCoroutine(Cooldown());

            if (dotTaken >= nbOfDots)
            {
                hasBleed = false;
                dotTaken = 0;
                isBleeding = false;
            }
        }
    }

    IEnumerator Cooldown()
    {
        hasBleed = true;
        yield return new WaitForSeconds(timeBetweenDots);
        Bleed();
        hasBleed = false;
    }

    void Bleed()
    {
        health -= dotDmgs;
        dotTaken++;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(health);
            stream.SendNext(isBleeding);
            stream.SendNext(die);
            stream.SendNext(countdown);
        }
        else if(!PhotonNetwork.IsMasterClient)
        {
            health = (int)stream.ReceiveNext();
            isBleeding = (bool)stream.ReceiveNext();
            die = (bool)stream.ReceiveNext();
            countdown = (float)stream.ReceiveNext();
        }
    }

    public void Death()
    {
        health -= damage;
    }

    public void InstantDeath()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0)
        {
            die = false;
            countdown = 5f;
            health = 0;
        }
    }

}
