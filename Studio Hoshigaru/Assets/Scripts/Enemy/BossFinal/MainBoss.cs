using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class MainBoss : MonoBehaviourPun
{
    public int step;
    public Stockage stock;
    public bool DPSing;
    public bool beginning;
    public float maxWait;
    public float maxWait1;
    public bool waiting;
    public bool waiting1;
    public bool endAttack;
    public int phase;
    public int MaxHp;
    public float laserSpeed;
    public float damageSpeed;
    public bool movingToLaser;
    public bool movingFromLaser;
    private float wait;
    private float wait1;
    public string facingDirection;
    public int timerDPSMax;
    private float timerDps;
    public bool movingToDamage;
    public bool movingFromDamage;

    private void Start()
    {
        timerDps = timerDPSMax;
        step = 0;
        endAttack = true;
        stock = GetComponent<Stockage>();
        phase = 1;
        waiting = true;
        wait1 = maxWait1;
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
            else if(waiting1)
            {
                Wait1();
            }
            else if(movingToLaser)
            {
                MoveLaser1();
            }
            else if (movingFromLaser)
            {
                MoveFromLaser(facingDirection);
            }
            else if (movingToDamage)
            {
                MoveToDamage();
            }
            else if(movingFromDamage)
            {
                MoveFromDamage();
            }
            else if(DPSing)
            {
                TakeHit();
            }
            else if(endAttack)
            {
                switch(step % 4)
                {
                    case 0:
                        TentacleBegin();
                        break;
                    case 1:
                        MeteorBegin();
                        break;
                    case 2:
                        LaserBegin();
                        break;
                    case 3:
                        Damageable();
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

    public void Wait1()
    {
        wait1 -= Time.deltaTime;
        if (wait1 <= 0)
        {
            wait1 = maxWait1;
            waiting1 = false;
        }
    }

    public void TakeHit()
    {
        timerDps -= Time.deltaTime;
        if(timerDps <= 0)
        {
            timerDps = timerDPSMax;
            DPSing = false;
            movingFromDamage = true;
            stock.DamageZone.SetActive(false);
        }
    }

    public void MoveToDamage()
    {
        if(Mathf.Abs(stock.BossComplet.transform.position.y - stock.DamageZone.transform.position.y) > 0.01f)
        {
            Vector2 targetPos = new Vector2(stock.BossComplet.transform.position.x, stock.DamageZone.transform.position.y);
            stock.BossComplet.transform.position = Vector2.MoveTowards(stock.BossComplet.transform.position, targetPos, Time.deltaTime * damageSpeed);
            stock.BossEyes.transform.position = Vector2.MoveTowards(stock.BossComplet.transform.position, targetPos, Time.deltaTime * damageSpeed);
        }
        else
        {
            movingToDamage = false;
            stock.DamageZone.SetActive(true);
            DPSing = true;
        }
    }

    public void MoveFromDamage()
    {
        if (Mathf.Abs(stock.BossComplet.transform.position.y - transform.position.y) > 0.01f)
        {
            Vector2 targetPos = new Vector2(stock.BossComplet.transform.position.x,transform.position.y);
            stock.BossComplet.transform.position = Vector2.MoveTowards(stock.BossComplet.transform.position, targetPos, Time.deltaTime * damageSpeed);
            stock.BossEyes.transform.position = Vector2.MoveTowards(stock.BossComplet.transform.position, targetPos, Time.deltaTime * damageSpeed);
        }
        else
        {
            movingFromDamage = false;
            endAttack = true;
            step +=1;
        }
    }

    public void Damageable()
    {
        endAttack = false;
        movingToDamage = true;
    }

    public void LaserBegin()
    {
        endAttack = false;
        if (phase == 1)
        {
            EyesAnim eyes = stock.BossEyes.GetComponent<PhaseChanging>().P1.GetComponent<EyesAnim>();
            eyes.boss = this;
            eyes.phase = 1;
            eyes.anim.SetBool("Laser", true);
        }
    }

    public void MoveLaser1()
    {
        if(step == 2)
        {
            if(Mathf.Abs(stock.BossSideL.transform.position.x - stock.LaserPlace1.transform.position.x)>0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.LaserPlace1.transform.position.x, stock.BossSideL.transform.position.y);
                stock.BossSideL.transform.position = Vector2.MoveTowards(stock.BossSideL.transform.position, targetPosition, laserSpeed * Time.deltaTime);
            }
            else if(Mathf.Abs(stock.BossSideL.transform.position.y - stock.LaserPlace1.transform.position.y) > 0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.BossSideL.transform.position.x, stock.LaserPlace1.transform.position.y);
                stock.BossSideL.transform.position = Vector2.MoveTowards(stock.BossSideL.transform.position, targetPosition, laserSpeed * Time.deltaTime);
            }
            else
            {
                movingToLaser = false;
                Shooting("right");
            }
        }
        else if (step == 6)
        {
          
            if(Mathf.Abs(stock.BossSideL.transform.position.x - stock.LaserPlace3.transform.position.x)>0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.LaserPlace3.transform.position.x, stock.BossSideL.transform.position.y);
                stock.BossSideL.transform.position = Vector2.MoveTowards(stock.BossSideL.transform.position, targetPosition, laserSpeed * Time.deltaTime);
            }
            else if(Mathf.Abs(stock.BossSideL.transform.position.y - stock.LaserPlace3.transform.position.y) > 0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.BossSideL.transform.position.x, stock.LaserPlace3.transform.position.y);
                stock.BossSideL.transform.position = Vector2.MoveTowards(stock.BossSideL.transform.position, targetPosition, laserSpeed * Time.deltaTime);
            }
            else
            {
                movingToLaser = false;
                Shooting("right");
            }
        }
        else if (step == 10)
        {

            if(Mathf.Abs(stock.BossSideR.transform.position.x - stock.LaserPlace2.transform.position.x)>0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.LaserPlace2.transform.position.x, stock.BossSideR.transform.position.y);
                stock.BossSideR.transform.position = Vector2.MoveTowards(stock.BossSideR.transform.position, targetPosition, laserSpeed * Time.deltaTime);
            }
            else if(Mathf.Abs(stock.BossSideR.transform.position.y - stock.LaserPlace2.transform.position.y)>0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.BossSideR.transform.position.x, stock.LaserPlace2.transform.position.y);
                stock.BossSideR.transform.position = Vector2.MoveTowards(stock.BossSideR.transform.position, targetPosition, laserSpeed * Time.deltaTime);
            }
            else
            {
                movingToLaser = false;
                Shooting("left");
            }
        }

    }
    public void MoveFromLaser(string facingdirection)
    {
        if (facingdirection == "left")
        {
            if (Mathf.Abs(stock.BossSideR.transform.position.y - stock.BossComplet.transform.position.y)>0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.BossSideR.transform.position.x, stock.BossComplet.transform.position.y);
                stock.BossSideR.transform.position = Vector2.MoveTowards(stock.BossSideR.transform.position, targetPosition, laserSpeed * Time.deltaTime);
            }
            else if (Mathf.Abs(stock.BossSideR.transform.position.x - stock.BossComplet.transform.position.x)>0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.BossComplet.transform.position.x, stock.BossSideR.transform.position.y);
                stock.BossSideR.transform.position = Vector2.MoveTowards(stock.BossSideR.transform.position, targetPosition, laserSpeed * Time.deltaTime);
            }
            else
            {
                movingFromLaser = false;
                LaserEndedR();
            }
        }
        else
        {
            if (Mathf.Abs(stock.BossSideL.transform.position.y - stock.BossComplet.transform.position.y)>0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.BossSideL.transform.position.x, stock.BossComplet.transform.position.y);
                stock.BossSideL.transform.position = Vector2.MoveTowards(stock.BossSideL.transform.position, targetPosition, laserSpeed * Time.deltaTime);
            }
            else if (Mathf.Abs(stock.BossSideL.transform.position.x - stock.BossComplet.transform.position.x)>0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.BossComplet.transform.position.x, stock.BossSideL.transform.position.y);
                stock.BossSideL.transform.position = Vector2.MoveTowards(stock.BossSideL.transform.position, targetPosition, laserSpeed * Time.deltaTime);
            }
            else
            {
                movingFromLaser = false;
                LaserEndedL();
            }
        }
    }

    public void Shooting(string facingdirection)
    {
        if (phase == 1)
        {
            if (step == 2)
            {
                GameObject l = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserDark"), stock.Laser1.transform.position, Quaternion.identity);
                Laser laser = l.GetComponent<Laser>();
                laser.facingDirection = facingdirection;
                laser.boss = this;
            }
            else if (step == 6)
            {
                GameObject l = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserDark"), stock.Laser3.transform.position, Quaternion.identity);
                Laser laser = l.GetComponent<Laser>();
                laser.facingDirection = facingdirection;
                laser.boss = this;
            }
            else if (step == 10)
            {
                GameObject l = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserDark"), stock.Laser2.transform.position, Quaternion.identity);
                Laser laser = l.GetComponent<Laser>();
                laser.facingDirection = facingdirection;
                laser.boss = this;
            }
        }
    }
    public void MovingHead1()
    {
        if(step == 2)
        {
            stock.BossComplet.SetActive(false);
            stock.BossSideL.SetActive(true);
            movingToLaser = true;
        }
        else if(step == 6)
        {
            stock.BossComplet.SetActive(false);
            stock.BossSideL.SetActive(true);
            movingToLaser = true;
        }
        else if(step == 10)
        {
            stock.BossComplet.SetActive(false);
            stock.BossSideR.SetActive(true);
            movingToLaser = true;
        }
    }

    public void LaserEndedL()
    {
        stock.BossSideL.SetActive(false);
        stock.BossComplet.SetActive(true);
        endAttack = true;
        waiting = true;
        step += 1;
    }

    public void LaserEndedR()
    {
        stock.BossSideR.SetActive(false);
        stock.BossComplet.SetActive(true);
        endAttack = true;
        waiting = true;
        step += 1;
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
            EyesAnim eyes = stock.BossEyes.GetComponent<PhaseChanging>().P1.GetComponent<EyesAnim>();
            eyes.boss = this;
            eyes.phase = 1;
            eyes.anim.SetBool("Tentacle", true);
        }
    }

    public void TentacleWarn1()
    {
        endAttack = true;
        waiting1 = true;
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
        endAttack = true;
        waiting = true;
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
            PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "DarkMeteor"), pos, Quaternion.Euler(0, 0, 90));
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LightMeteor"), pos, Quaternion.Euler(0, 0, 90));
        }
    }
}
