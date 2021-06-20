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
    private void Start()
    {
        hasBleed = false;
        isBleeding = false;
        dotTaken = 0;
        countdown = 5f;
    }

    private void Update()
    {
        if(die)
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
        if (stream.IsWriting)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                stream.SendNext(health);
                stream.SendNext(isBleeding);
            }   
        }
        else
        {
            health = (int)stream.ReceiveNext();
            isBleeding = (bool)stream.ReceiveNext();
        }
    }

    public void Death()
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
