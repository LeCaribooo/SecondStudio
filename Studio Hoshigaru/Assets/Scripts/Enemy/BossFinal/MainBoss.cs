using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class MainBoss : MonoBehaviourPun
{
    public int step;
    public Stockage stock;
    public bool beginning;
    public float maxWait;
    public bool waiting;
    public bool endAttack;
    public int phase;
    public int MaxHp;

    private float wait;

    private void Start()
    {
        step = 0;
        endAttack = true;
        stock = GetComponent<Stockage>();
        phase = 1;
        waiting = true;
        wait = maxWait;
        stock.DamageZone.GetComponent<EnemyHealth>().health = MaxHp;
    }
    private void Update()
    {
        if(beginning)
        {
            step = step % 12;
            if(stock.DamageZone.GetComponent<EnemyHealth>().health <= 0)
            {
                if(phase == 1)
                {
                    //PhaseSwitch();
                }
                else
                {
                    //Death();
                }
            }
            else if(waiting)
            {
                Wait();
            }
            else if(endAttack)
            {
                switch(step % 4)
                {
                    case 0:
                        TentacleBegin();
                        break;
                    case 1:
                        //MeteorSpawn();
                        break;
                    case 2:
                        //Lasering();
                        break;
                    case 3:
                        //Damageable();
                        break;
                }
            }
        }
    }

    public void Wait()
    {
        wait -= Time.deltaTime;
        if(wait <= 0)
        {
            wait = maxWait;
            waiting = false;
        }
    }

    public void MeteorBegin()
    {
        endAttack = false;
        if (phase == 1)
        {
            EyesAnim eyes = stock.BossEyes.GetComponent<PhaseChanging>().P1.GetComponent<EyesAnim>();
            eyes.boss = this;
            eyes.phase = 1;
            eyes.anim.SetBool("Meteor", true);
        }
    }

    public void TentacleBegin()
    {
        endAttack = false;
        if (phase == 1)
        {
            Debug.Log("step1");
            EyesAnim eyes = stock.BossEyes.GetComponent<PhaseChanging>().P1.GetComponent<EyesAnim>();
            eyes.boss = this;
            eyes.phase = 1;
            eyes.anim.SetBool("Tentacle", true);
        }
    }

    public void TentacleWarn1()
    {
        endAttack = true;
        waiting = true;
        if(step == 0)
        {
            Warnings warn1 = stock.Tentacule2.GetComponent<Warnings>();
            Warnings warn2 = stock.Tentacule4.GetComponent<Warnings>();
            Warnings warn3 = stock.Tentacule6.GetComponent<Warnings>();
            Warnings warn4 = stock.Tentacule7.GetComponent<Warnings>();
            warn1.boss = this;
            warn2.boss = this;
            warn3.boss = this;
            warn4.boss = this;
            warn1.warn = true;
            warn2.warn = true;
            warn3.warn = true;
            warn4.warn = true;
            warn1.type = 1;
            warn2.type = 1;
            warn3.type = 1;
            warn4.type = 1;
            step += 1;
        }
        else if(step == 4)
        {
            Warnings warn1 = stock.Tentacule1.GetComponent<Warnings>();
            Warnings warn2 = stock.Tentacule3.GetComponent<Warnings>();
            Warnings warn3 = stock.Tentacule5.GetComponent<Warnings>();
            warn1.boss = this;
            warn2.boss = this;
            warn3.boss = this;
            warn1.warn = true;
            warn2.warn = true;
            warn3.warn = true;
            warn1.type = 1;
            warn2.type = 1;
            warn3.type = 1;
            step += 1;
        }
        else if(step == 8)
        {
            Warnings warn1 = stock.Tentacule3.GetComponent<Warnings>();
            Warnings warn2 = stock.Tentacule5.GetComponent<Warnings>();
            Warnings warn3 = stock.Tentacule6.GetComponent<Warnings>();
            warn1.boss = this;
            warn2.boss = this;
            warn3.boss = this;
            warn1.warn = true;
            warn2.warn = true;
            warn3.warn = true;
            warn1.type = 1;
            warn2.type = 1;
            warn3.type = 1;
            step += 1;
        }
    }

    public void MeteorWarn1()
    {
        if (step == 1)
        {
            Warnings warn1 = stock.Meteor1.GetComponent<Warnings>();
            Warnings warn2 = stock.Meteor4.GetComponent<Warnings>();
            Warnings warn3 = stock.Meteor6.GetComponent<Warnings>();
            warn1.boss = this;
            warn2.boss = this;
            warn3.boss = this;
            warn1.warn = true;
            warn2.warn = true;
            warn3.warn = true;
            warn1.type = 2;
            warn2.type = 2;
            warn3.type = 2;
            step += 1;
        }
        else if (step == 5)
        {
            Warnings warn1 = stock.Meteor2.GetComponent<Warnings>();
            Warnings warn2 = stock.Meteor5.GetComponent<Warnings>();
            Warnings warn3 = stock.Meteor7.GetComponent<Warnings>();
            Warnings warn4 = stock.Meteor8.GetComponent<Warnings>();
            warn1.boss = this;
            warn2.boss = this;
            warn3.boss = this;
            warn4.boss = this;
            warn1.warn = true;
            warn2.warn = true;
            warn3.warn = true;
            warn4.warn = true;
            warn1.type = 2;
            warn2.type = 2;
            warn3.type = 2;
            warn4.type = 2;
            step += 1;
        }
        else if(step == 9)
        {
            Warnings warn1 = stock.Meteor1.GetComponent<Warnings>();
            Warnings warn2 = stock.Meteor2.GetComponent<Warnings>();
            Warnings warn3 = stock.Meteor3.GetComponent<Warnings>();
            Warnings warn5 = stock.Meteor5.GetComponent<Warnings>();
            Warnings warn4 = stock.Meteor8.GetComponent<Warnings>();
            warn1.boss = this;
            warn2.boss = this;
            warn3.boss = this;
            warn4.boss = this;
            warn5.boss = this;
            warn1.warn = true;
            warn2.warn = true;
            warn3.warn = true;
            warn4.warn = true;
            warn5.warn = true;
            warn1.type = 2;
            warn2.type = 2;
            warn3.type = 2;
            warn4.type = 2;
            warn5.type = 2;
            step += 1;
        }
    }

    public void TentacleSpawn(Vector3 pos)
    {
        if (phase == 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "DarkTentacle"), pos, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LightTentacle"), pos, Quaternion.identity);
        }
    }

    public void MeteorSpawn(Vector3 pos)
    {
        if (phase == 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "DarkMeteor"), pos, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LightMeteor"), pos, Quaternion.identity);
        }
    }
}
