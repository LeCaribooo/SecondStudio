using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.Experimental.Rendering.Universal;

public class MainBoss : MonoBehaviourPun, IPunObservable
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
    public float wait;
    public float wait1;
    public string facingDirection;
    public int timerDPSMax;
    private float timerDps;
    public bool movingToDamage;
    public bool movingFromDamage;
    public float moveSpeed;
    public bool moving;
    public bool illuminating;
    public bool flash;
    public bool movingback;
    public bool falling;
    public bool changing;
    public bool augmente;
    public bool check;
    public bool dead;
    public bool blinding;
    public bool aveugle;
    public float intensity1;
    Light2D light;
    EyesAnim eyes1;
    EyesAnim eyes2;
    Warnings warn1;
    Warnings warn2;
    Warnings warn3;
    Warnings warn4;
    Warnings warn5;
    Warnings warn6;
    Warnings warn7;

    private void Start()
    {
        augmente = true;
        timerDps = timerDPSMax;
        step = 0;
        endAttack = true;
        stock = GetComponent<Stockage>();
        phase = 1;
        waiting = true;
        wait1 = maxWait1;
        wait = maxWait;
        base.photonView.RPC("HPSet", RpcTarget.All, MaxHp);
        base.photonView.RPC("GetLight", RpcTarget.All);
    }
    private void Update()
    { 
        if(beginning)
        {
            step = step % 12;
            if(flash)
            {
                FlashBang();
            }
            else if(blinding)
            {
                Blind();
            }
            else if(aveugle)
            {
                Aveuglement();
            }
            else if(moving)
            {
                moving = DeadMove1();
                if(!moving)
                {
                    check = true;
                    waiting1 = true;
                }
            }
            else if(falling)
            {
                falling = DeadMove2();
                if(!falling && phase != 3)
                {
                    wait = maxWait * 1.5f;
                    waiting = true;
                }
            }
            else if(movingback)
            {
                movingback = MoveBack1();
                if(!movingback)
                {
                    base.photonView.RPC("LActive", RpcTarget.All, false);
                    base.photonView.RPC("RActive", RpcTarget.All, false);
                    base.photonView.RPC("CompletActive", RpcTarget.All, true);
                    stock.BossComplet.GetComponent<PhaseChanging>().p2 = true;
                    base.photonView.RPC("BCBossP2", RpcTarget.All);
                }
            }
            else if(illuminating)
            {
                 LightPhase();
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
                if (phase == 1)
                {
                    MoveLaser1();
                }
                else
                {
                    MoveLaser2();
                }
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
            else if (stock.DamageZone.GetComponent<EnemyHealth>().health <= 0 && !changing)
            {
                if (phase == 1)
                {
                    endAttack = false;
                    changing = true;
                    base.photonView.RPC("BossCP1", RpcTarget.All, false);
                    base.photonView.RPC("LActive", RpcTarget.All, true);
                    base.photonView.RPC("RActive", RpcTarget.All, true);
                    PhaseSwitch();
                }
                else
                {
                    Death();
                }
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

    public void Aveuglement()
    {

        if (augmente)
        {
            intensity1 += 0.01f;
            light.intensity = intensity1;
        }
        else
        {
            intensity1 -= 0.01f;
            light.intensity = intensity1;
        }
        if (intensity1 >= 2 && augmente)
        {
            base.photonView.RPC("EyesActive", RpcTarget.All, false);
            stock.SmokeCou.GetComponent<PhaseChanging>().P1.GetComponent<Animator>().SetBool("Reduce", true);
            stock.SmokeTorse.GetComponent<PhaseChanging>().P1.GetComponent<Animator>().SetBool("Reduce", true);
            base.photonView.RPC("CompletActive", RpcTarget.All, false);
            base.photonView.RPC("P1", RpcTarget.All, false);
            base.photonView.RPC("L1", RpcTarget.All, true);
            base.photonView.RPC("R1", RpcTarget.All, true);
            base.photonView.RPC("H1", RpcTarget.All, true);
            base.photonView.RPC("SmokeHActive", RpcTarget.All, false);
            base.photonView.RPC("SmokeLActive", RpcTarget.All, false);
            base.photonView.RPC("SmokeRActive", RpcTarget.All, false);
            augmente = false;
        }
        else if (intensity1 <= 0.6f && !augmente)
        {
            aveugle = false;
            moving = true;
            augmente = true;
            light.intensity = 0.6f;
            intensity1 = 0.6f;
        }
    }

    public void Blind()
    {
        if (augmente)
        {
            intensity1 += 0.01f;
            light.intensity = intensity1;
        }
        else
        {
            intensity1 -= 0.01f;
            light.intensity = intensity1;
        }
        if (intensity1 >= 2 && augmente)
        {
            base.photonView.RPC("EyesActive", RpcTarget.All, true);
            base.photonView.RPC("CouP2", RpcTarget.All, true);
            base.photonView.RPC("TorseP2", RpcTarget.All, true);
            base.photonView.RPC("L2", RpcTarget.All, true);
            base.photonView.RPC("R2", RpcTarget.All, true);
            base.photonView.RPC("H2", RpcTarget.All, true);
            base.photonView.RPC("SmokeHActive", RpcTarget.All, true);
            base.photonView.RPC("SmokeLActive", RpcTarget.All, true);
            base.photonView.RPC("SmokeRActive", RpcTarget.All, true);
            base.photonView.RPC("HPSet", RpcTarget.All, MaxHp);
            augmente = false;
        }

        else if (intensity1 <= 0.6f && !augmente)
        {
            blinding = false;
            light.intensity = 0.6f;
            intensity1 = 0.06f;
            changing = false;
            waiting = true;
            phase += 1;
            step = 0;
            endAttack = true;
            augmente = true;
        }
    }

    public void Death()
    {
        phase = 3;
        base.photonView.RPC("EyesActive", RpcTarget.All, false);
        base.photonView.RPC("DeaD", RpcTarget.All);
        stock.BossComplet.GetComponent<PhaseChanging>().P3.GetComponent<Animator>().SetBool("Dead", true);
    }

    public void Empty()
    {
        base.photonView.RPC("CompletActive", RpcTarget.All,false );
        base.photonView.RPC("LActive", RpcTarget.All, true);
        base.photonView.RPC("RActive", RpcTarget.All, true);
        flash = true;
        Clear();
    }

    public void FlashBang()
    {
        if (augmente)
        {
            intensity1 += 0.01f;
            light.intensity = intensity1;
        }
        else
        {
            intensity1 -= 0.01f;
            light.intensity = intensity1;
        }
        if (intensity1 >= 5 && augmente)
        {
            base.photonView.RPC("P2", RpcTarget.All, false);
            base.photonView.RPC("L3", RpcTarget.All, true);
            base.photonView.RPC("R3", RpcTarget.All, true);
            base.photonView.RPC("H3", RpcTarget.All, true);
            stock.SmokeCou.GetComponent<PhaseChanging>().P2.GetComponent<Animator>().SetBool("Reduce", true);
            stock.SmokeTorse.GetComponent<PhaseChanging>().P2.GetComponent<Animator>().SetBool("Reduce", true);
            base.photonView.RPC("SmokeHActive", RpcTarget.All, false);
            base.photonView.RPC("SmokeLActive", RpcTarget.All, false);
            base.photonView.RPC("SmokeRActive", RpcTarget.All, false);
            stock.BossL.GetComponent<PhaseChanging>().p1 = true;
            stock.BossR.GetComponent<PhaseChanging>().p1 = true;
            augmente = false;
        }
        else if (intensity1 <= 0.6f && !augmente)
        {
            augmente = true;
            light.intensity = 0.6f;
            intensity1 = 0.6f;
            flash = false;
            dead = true;
            waiting = true;
        }
    }

    public void LightPhase()
    {
        if (augmente)
        {
            intensity1 += 0.01f;
            light.intensity = intensity1;
        }
        else
        {
            intensity1 -= 0.01f;
            light.intensity = intensity1;
        }
        if (intensity1 >= 5 && augmente)
        {
            stock.BossL.GetComponent<PhaseChanging>().p2 = true;
            stock.BossR.GetComponent<PhaseChanging>().p2 = true;
            stock.BossSideL.GetComponent<PhaseChanging>().p2 = true;
            stock.BossSideR.GetComponent<PhaseChanging>().p2 = true;
            stock.BossEyes.GetComponent<PhaseChanging>().p2 = true;
            augmente = false;
        }
        else if(intensity1 <= 0.6f && !augmente)
        {
            augmente = true;
            light.intensity = 0.6f;
            intensity1 = 0.6f;
            illuminating = false;
            movingback = true;
        }
    }

    public void EndChange()
    {
        blinding = true;
    }

    public bool MoveBack1()
    {
        bool result = true;
        if (Mathf.Abs(stock.BossL.transform.position.y - stock.BossComplet.transform.position.y) > 0.01f)
        {
            Vector2 targetPos = new Vector2(stock.BossL.transform.position.x, stock.BossComplet.transform.position.y);
            stock.BossL.transform.position = Vector2.MoveTowards(stock.BossL.transform.position, targetPos, Time.deltaTime * damageSpeed);
            
        }
        if (Mathf.Abs(stock.BossR.transform.position.y - stock.BossComplet.transform.position.y) > 0.01f)
        {
            Vector2 targetPos = new Vector2(stock.BossR.transform.position.x, stock.BossComplet.transform.position.y);
            stock.BossR.transform.position = Vector2.MoveTowards(stock.BossR.transform.position, targetPos, Time.deltaTime * damageSpeed);
            
        }
        else
        {
            result = false;
        }
        return result;
    }

    public bool MoveBackR()
    {
        if (Mathf.Abs(stock.BossR.transform.position.y - stock.BossComplet.transform.position.y) > 0.01f)
        {
            Vector2 targetPos = new Vector2(stock.BossR.transform.position.x, stock.BossComplet.transform.position.y);
            stock.BossR.transform.position = Vector2.MoveTowards(stock.BossR.transform.position, targetPos, Time.deltaTime * damageSpeed);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Clear()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] boss = GameObject.FindGameObjectsWithTag("Boss");
        for (int i = boss.Length-1; i >= 0; i--)
        {
            boss[i].GetComponent<EnemyHealth>().die = true;
        }
        for (int j = enemy.Length-1; j >= 0; j--)
        {
            enemy[j].GetComponent<EnemyHealth>().die = true;
        }
    }
    public void PhaseSwitch()
    {
        aveugle = true;
        Clear();
    }

    public bool DeadMove2()
    {
        bool result = false;
        if (Mathf.Abs(stock.BossR.transform.position.y - stock.deadR.transform.position.y) > 0.01f)
        {
            Vector2 targetPos = new Vector2(stock.deadR.transform.position.x, stock.deadR.transform.position.y);
            stock.BossR.transform.position = Vector2.MoveTowards(stock.BossR.transform.position, targetPos, Time.deltaTime * moveSpeed * 25);
            result = true;
        }
        if (Mathf.Abs(stock.BossL.transform.position.y - stock.deadL.transform.position.y) > 0.01f)
        {
            Vector2 targetPos = new Vector2(stock.deadL.transform.position.x, stock.deadL.transform.position.y);
            stock.BossL.transform.position = Vector2.MoveTowards(stock.BossL.transform.position, targetPos, Time.deltaTime * moveSpeed * 25);
            result = true;
        }
        return result;
    }

    public bool DeadMove1()
    {
        bool result = false;
        if (Mathf.Abs(stock.BossR.transform.position.x - stock.deadR.transform.position.x) > 0.01f)
        {
            Vector2 targetPos = new Vector2(stock.deadR.transform.position.x, stock.BossR.transform.position.y);
            stock.BossR.transform.position = Vector2.MoveTowards(stock.BossR.transform.position, targetPos, Time.deltaTime * moveSpeed);
            result = true;
        }
        if (Mathf.Abs(stock.BossL.transform.position.x - stock.deadL.transform.position.x) > 0.01f)
        {
            Vector2 targetPos = new Vector2(stock.deadL.transform.position.x, stock.BossL.transform.position.y);
            stock.BossL.transform.position = Vector2.MoveTowards(stock.BossL.transform.position, targetPos, Time.deltaTime * moveSpeed);
            result = true;
        }
        return result;
    }
    public void Wait()
    {
        wait -= Time.deltaTime;
        if(wait <= 0)
        {
            wait = maxWait;
            if(check)
            {
                illuminating = true;
                check = false;
            }
            else if(dead)
            {
                dead = false;
                falling = true;
            }
            waiting = false;
            
        }
    }

    public void Wait1()
    {
        wait1 -= Time.deltaTime;
        if (wait1 <= 0)
        {
            wait1 = maxWait1;
            if(check)
            {
                falling = true;
            }
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
            base.photonView.RPC("ActiveDamage", RpcTarget.All, false);
        }
    }

    public void MoveToDamage()
    {
        if(Mathf.Abs(stock.BossComplet.transform.position.y - stock.DamageZone.transform.position.y) > 0.01f)
        {
            Vector2 targetPos = new Vector2(stock.BossComplet.transform.position.x, stock.DamageZone.transform.position.y);
            stock.BossComplet.transform.position = Vector2.MoveTowards(stock.BossComplet.transform.position, targetPos, Time.deltaTime * damageSpeed);
            stock.BossEyes.transform.position = Vector2.MoveTowards(stock.BossComplet.transform.position, targetPos, Time.deltaTime * damageSpeed);
            stock.SmokeCou.transform.position = Vector2.MoveTowards(stock.SmokeCou.transform.position, stock.smokePlace.transform.position, Time.deltaTime * damageSpeed);
        }
        else
        {
            movingToDamage = false;
            int i = GameObject.FindGameObjectsWithTag("Player").Length;
            Transform[] spawnshini = {stock.shinigami1.transform, stock.shinigami2.transform, stock.shinigami3.transform, stock.shinigami4.transform, stock.shinigami5.transform, stock.shinigami6.transform, stock.shinigami7.transform, stock.shinigami8.transform };
            Transform[] spawndark = {stock.dark1.transform, stock.dark2.transform, stock.dark3.transform, stock.dark4.transform, stock.dark5.transform, stock.dark6.transform, stock.dark7.transform, stock.dark8.transform};
            if (phase == 1)
            {
                if(step == 3 || step == 11)
                {
                    for(int j = 0; j < i*2;j += 2)
                    {
                        PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "shinigami"), spawnshini[j].position, Quaternion.identity);
                        PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss" ,"DarkSpiritBoss"), spawndark[j].position, Quaternion.identity);
                    }
                    
                }
                else if(step == 7)
                {
                    for(int j = 1; j < i*2; j+=2)
                    {
                        PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "shinigami"), spawnshini[j].position, Quaternion.identity);
                        PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss" ,"DarkSpiritBoss"), spawndark[j].position, Quaternion.identity);
                    }

                }
            }
            else
            {
                for (int j = 0; j < i * 2; j++)
                {
                    PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "shinigamiLight"), spawnshini[j].position, Quaternion.identity);
                    PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss" ,"LightSpiritBoss"), spawndark[j].position, Quaternion.identity);
                }
            }
            base.photonView.RPC("ActiveDamage", RpcTarget.All, true);
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
            stock.SmokeCou.transform.position = Vector2.MoveTowards(stock.SmokeCou.transform.position, stock.smokePlace.transform.position, Time.deltaTime * damageSpeed);
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
            base.photonView.RPC("Eyes1", RpcTarget.All);
            base.photonView.RPC("Eyes1B", RpcTarget.All);
            eyes1.phase = 1;
            eyes1.anim.SetBool("Laser", true);
        }
        else
        {
            base.photonView.RPC("Eyes2", RpcTarget.All);
            base.photonView.RPC("Eyes2B", RpcTarget.All);
            eyes2.phase = 1;
            eyes2.anim.SetBool("Laser", true);
        }
    }

    public void MoveLaser2()
    {
        if (step == 2 || step == 10)
        {
            if (Mathf.Abs(stock.BossSideL.transform.position.x - stock.LaserPlace2bis.transform.position.x) > 0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.LaserPlace2bis.transform.position.x, stock.BossSideL.transform.position.y);
                stock.BossSideL.transform.position = Vector2.MoveTowards(stock.BossSideL.transform.position, targetPosition, laserSpeed * Time.deltaTime);
            }
            else if (Mathf.Abs(stock.BossSideL.transform.position.y - stock.LaserPlace2bis.transform.position.y) > 0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.BossSideL.transform.position.x, stock.LaserPlace2bis.transform.position.y);
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

            if (Mathf.Abs(stock.BossSideR.transform.position.x - stock.LaserPlace2.transform.position.x) > 0.01f)
            {
                Vector2 targetPosition = new Vector2(stock.LaserPlace2.transform.position.x, stock.BossSideR.transform.position.y);
                stock.BossSideR.transform.position = Vector2.MoveTowards(stock.BossSideR.transform.position, targetPosition, laserSpeed * Time.deltaTime);
            }
            else if (Mathf.Abs(stock.BossSideR.transform.position.y - stock.LaserPlace2.transform.position.y) > 0.01f)
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
            }
            else if (step == 6)
            {
                GameObject l = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserDark"), stock.Laser3.transform.position, Quaternion.identity);
                Laser laser = l.GetComponent<Laser>();
                laser.facingDirection = facingdirection;
            }
            else if (step == 10)
            {
                GameObject l = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserDark"), stock.Laser2.transform.position, Quaternion.identity);
                Laser laser = l.GetComponent<Laser>();
                laser.facingDirection = facingdirection;
            }
        }
        else
        {
            if (step == 2)
            {
                GameObject l = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserLight"), stock.Laser1.transform.position, Quaternion.identity);
                GameObject l2 = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserLight"), stock.Laser2.transform.position, Quaternion.identity);
                Laser laser = l.GetComponent<Laser>();
                Laser laser2 = l2.GetComponent<Laser>();
                laser.facingDirection = facingdirection;
                laser2.facingDirection = facingdirection;
            }
            else if (step == 6)
            {
                Debug.Log("3");
                GameObject l = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserLight"), stock.Laser3.transform.position, Quaternion.identity);
                GameObject l2 = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserLight"), stock.Laser2.transform.position, Quaternion.identity);
                Laser laser = l.GetComponent<Laser>();
                Laser laser2 = l2.GetComponent<Laser>();
                laser.facingDirection = facingdirection;
                laser2.facingDirection = facingdirection;
            }
            else if (step == 10)
            {
                GameObject l = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserLight"), stock.Laser1.transform.position, Quaternion.identity);
                GameObject l2 = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserLight"), stock.Laser3.transform.position, Quaternion.identity);
                Laser laser = l.GetComponent<Laser>();
                Laser laser2 = l2.GetComponent<Laser>();
                laser.facingDirection = facingdirection;
                laser2.facingDirection = facingdirection;
            }
        }
    }
    public void MovingHead()
    {
        if (phase == 1)
        {
            if (step == 2)
            {
                base.photonView.RPC("CompletActive", RpcTarget.All, false);
                base.photonView.RPC("SLActive", RpcTarget.All, true);
                movingToLaser = true;
            }
            else if (step == 6)
            {
                base.photonView.RPC("CompletActive", RpcTarget.All, false);
                base.photonView.RPC("SLActive", RpcTarget.All, true);
                movingToLaser = true;
            }
            else if (step == 10)
            {
                base.photonView.RPC("CompletActive", RpcTarget.All, false);
                base.photonView.RPC("SRActive", RpcTarget.All, true);
                movingToLaser = true;
            }
        }
        else
        {
            if (step == 2 || step == 10)
            {
                base.photonView.RPC("CompletActive", RpcTarget.All, false);
                base.photonView.RPC("SLActive", RpcTarget.All, true);
                movingToLaser = true;
            }
            else
            {
                base.photonView.RPC("CompletActive", RpcTarget.All, false);
                base.photonView.RPC("SRActive", RpcTarget.All, true);
                movingToLaser = true;
            }
        }
    }

    public void LaserEndedL()
    {
        base.photonView.RPC("SLActive", RpcTarget.All, false);
        base.photonView.RPC("CompletActive", RpcTarget.All, true);
        waiting = true;
        endAttack = true;
        step += 1;
    }

    public void LaserEndedR()
    {
        base.photonView.RPC("SRActive", RpcTarget.All, false);
        base.photonView.RPC("CompletActive", RpcTarget.All, true);
        waiting = true;
        endAttack = true;
        step += 1;
    }

    public void MeteorBegin()
    {
        endAttack = false;
        if (phase == 1)
        {
            base.photonView.RPC("Eyes1", RpcTarget.All);
            base.photonView.RPC("Eyes1B", RpcTarget.All);
            eyes1.phase = 1;
            eyes1.anim.SetBool("Meteor", true);
        }
        else
        {
            base.photonView.RPC("Eyes2", RpcTarget.All);
            base.photonView.RPC("Eyes2B", RpcTarget.All);
            eyes2.phase = 1;
            eyes2.anim.SetBool("Meteor", true);
        }
    }

    public void TentacleBegin()
    {
        endAttack = false;
        if (phase == 1)
        {
            base.photonView.RPC("Eyes1", RpcTarget.All);
            base.photonView.RPC("Eyes1B", RpcTarget.All);
            eyes1.phase = 1;
            eyes1.anim.SetBool("Tentacle", true);
        }
        else
        {
            base.photonView.RPC("Eyes2", RpcTarget.All);
            base.photonView.RPC("Eyes2B", RpcTarget.All);
            eyes2.phase = 1;
            eyes2.anim.SetBool("Tentacle", true);
        }
    }

    public void TentacleWarn()
    {

        endAttack = true;
        waiting1 = true;
        if (phase == 1)
        {
            if (step == 0)
            {
                warn5 = stock.Tentacule1.GetComponent<Warnings>();
                warn1 = stock.Tentacule2.GetComponent<Warnings>();
                warn2 = stock.Tentacule4.GetComponent<Warnings>();
                warn3 = stock.Tentacule6.GetComponent<Warnings>();
                warn4 = stock.Tentacule7.GetComponent<Warnings>();
                warn1.warn = true;
                warn2.warn = true;
                warn3.warn = true;
                warn4.warn = true;
                warn5.warn = true;
                warn1.type = 1;
                warn2.type = 1;
                warn3.type = 1;
                warn4.type = 1;
                warn5.type = 1;
                step += 1;
            }
            else if (step == 4)
            {
                warn1 = stock.Tentacule1.GetComponent<Warnings>();
                warn2 = stock.Tentacule3.GetComponent<Warnings>();
                warn3 = stock.Tentacule5.GetComponent<Warnings>();
                warn4 = stock.Tentacule4.GetComponent<Warnings>();
                warn5 = stock.Tentacule7.GetComponent<Warnings>();
                warn1.warn = true;
                warn2.warn = true;
                warn3.warn = true;
                warn4.warn = true;
                warn5.warn = true;
                warn1.type = 1;
                warn2.type = 1;
                warn3.type = 1;
                warn4.type = 1;
                warn5.type = 1;
                step += 1;
            }
            else if (step == 8)
            {
                warn1 = stock.Tentacule3.GetComponent<Warnings>();
                warn2 = stock.Tentacule5.GetComponent<Warnings>();
                warn3 = stock.Tentacule6.GetComponent<Warnings>();
                warn4 = stock.Tentacule4.GetComponent<Warnings>();
                warn5 = stock.Tentacule1.GetComponent<Warnings>();
                warn1.warn = true;
                warn2.warn = true;
                warn3.warn = true;
                warn4.warn = true;
                warn5.warn = true;
                warn1.type = 1;
                warn2.type = 1;
                warn3.type = 1;
                warn4.type = 1;
                warn5.type = 1;
                step += 1;
            }
        }
        else
        {

            warn1 = stock.Tentacule1.GetComponent<Warnings>();
            warn2 = stock.Tentacule2.GetComponent<Warnings>();
            warn3 = stock.Tentacule3.GetComponent<Warnings>();
            warn4 = stock.Tentacule4.GetComponent<Warnings>();
            warn5 = stock.Tentacule5.GetComponent<Warnings>();
            warn6 = stock.Tentacule6.GetComponent<Warnings>();
            warn7 = stock.Tentacule7.GetComponent<Warnings>();
            warn1.warn = true;
            warn2.warn = true;
            warn3.warn = true;
            warn4.warn = true;
            warn5.warn = true;
            warn6.warn = true;
            warn7.warn = true;
            warn1.type = 1;
            warn2.type = 1;
            warn3.type = 1;
            warn4.type = 1;
            warn5.type = 1;
            warn6.type = 1;
            warn7.type = 1;
            step += 1;
        }
    }

    public void MeteorWarn()
    {
        endAttack = true;
        waiting = true;
        if (phase == 1)
        {
            if (step == 1)
            {
                warn1 = stock.Meteor1.GetComponent<Warnings>();
                warn2 = stock.Meteor4.GetComponent<Warnings>();
                warn3 = stock.Meteor6.GetComponent<Warnings>();
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
                warn1 = stock.Meteor2.GetComponent<Warnings>();
                warn2 = stock.Meteor5.GetComponent<Warnings>();
                warn3 = stock.Meteor7.GetComponent<Warnings>();
                warn4 = stock.Meteor8.GetComponent<Warnings>();
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
            else if (step == 9)
            {
                warn1 = stock.Meteor1.GetComponent<Warnings>();
                warn2 = stock.Meteor2.GetComponent<Warnings>();
                warn3 = stock.Meteor3.GetComponent<Warnings>();
                warn5 = stock.Meteor5.GetComponent<Warnings>();
                warn4 = stock.Meteor8.GetComponent<Warnings>();
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
        else
        {
            if(step == 1)
            {
                warn1 = stock.Meteor1.GetComponent<Warnings>();
                warn2 = stock.Meteor2.GetComponent<Warnings>();
                warn3 = stock.Meteor3.GetComponent<Warnings>();
                warn5 = stock.Meteor5.GetComponent<Warnings>();
                warn6 = stock.Meteor6.GetComponent<Warnings>();
                warn4 = stock.Meteor8.GetComponent<Warnings>();
                warn1.warn = true;
                warn2.warn = true;
                warn3.warn = true;
                warn4.warn = true;
                warn5.warn = true;
                warn6.warn = true;
                warn1.type = 2;
                warn2.type = 2;
                warn3.type = 2;
                warn4.type = 2;
                warn5.type = 2;
                warn6.type = 2;
                step += 1;
            }
            else if (step == 5)
            {
                warn1 = stock.Meteor1.GetComponent<Warnings>();
                warn2 = stock.Meteor7.GetComponent<Warnings>();
                warn3 = stock.Meteor3.GetComponent<Warnings>();
                warn5 = stock.Meteor4.GetComponent<Warnings>();
                warn6 = stock.Meteor6.GetComponent<Warnings>();
                warn4 = stock.Meteor8.GetComponent<Warnings>();
                warn1.warn = true;
                warn2.warn = true;
                warn3.warn = true;
                warn4.warn = true;
                warn5.warn = true;
                warn6.warn = true;
                warn1.type = 2;
                warn2.type = 2;
                warn3.type = 2;
                warn4.type = 2;
                warn5.type = 2;
                warn6.type = 2;
                step += 1;
            }
            else if (step == 9)
            {
                warn1 = stock.Meteor1.GetComponent<Warnings>();
                warn2 = stock.Meteor2.GetComponent<Warnings>();
                warn3 = stock.Meteor4.GetComponent<Warnings>();
                warn5 = stock.Meteor5.GetComponent<Warnings>();
                warn6 = stock.Meteor7.GetComponent<Warnings>();
                warn4 = stock.Meteor8.GetComponent<Warnings>();
                warn1.warn = true;
                warn2.warn = true;
                warn3.warn = true;
                warn4.warn = true;
                warn5.warn = true;
                warn6.warn = true;
                warn1.type = 2;
                warn2.type = 2;
                warn3.type = 2;
                warn4.type = 2;
                warn5.type = 2;
                warn6.type = 2;
                step += 1;
            }
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
    [PunRPC]
    public void BCBossP2()
    {
        stock.BossComplet.GetComponent<PhaseChanging>().P2.GetComponent<StateBoss>().boss = this;
    }
    [PunRPC]
    public void HPSet(int hp)
    {
        stock.DamageZone.GetComponent<EnemyHealth>().health = hp;
    }

    [PunRPC]
    public void CompletActive(bool active)
    {
        stock.BossComplet.SetActive(active);
    }
    [PunRPC]
    public void LActive(bool active)
    {
        stock.BossL.SetActive(active);
    }
    [PunRPC]
    public void RActive(bool active)
    {
        stock.BossR.SetActive(active);
    }
    [PunRPC]
    public void SLActive(bool active)
    {
        stock.BossSideL.SetActive(active);
    }
    [PunRPC]
    public void SRActive(bool active)
    {
        stock.BossSideL.SetActive(active);
    }
    [PunRPC]
    public void SmokeTActive(bool active)
    {
        stock.SmokeTorse.SetActive(active);
    }
    [PunRPC]
    public void SmokeCActive(bool active)
    {
        stock.SmokeCou.SetActive(active);
    }
    [PunRPC]
    public void DamageActive(bool active)
    {
        stock.DamageZone.SetActive(active);
    }
    [PunRPC]
    public void EyesActive(bool active)
    {
        stock.BossEyes.SetActive(active);
    }
    [PunRPC]
    public void SmokeLActive(bool active)
    {
        stock.SmokeL.SetActive(active);
    }
    [PunRPC]
    public void SmokeRActive(bool active)
    {
        stock.SmokeR.SetActive(active);
    }
    [PunRPC]
    public void SmokeHActive(bool active)
    {
        stock.SmokeH.SetActive(active);
    }
    [PunRPC]
    public void BossCP1(bool active)
    {
        stock.BossComplet.GetComponent<PhaseChanging>().P1.SetActive(active);
    }

    [PunRPC]
    public void P2(bool active)
    {
        stock.SmokeBras.GetComponent<PhaseChangingBras>().P2.SetActive(active);
    }
    [PunRPC]
    public void P1(bool active)
    {
        stock.SmokeBras.GetComponent<PhaseChangingBras>().P1.SetActive(active);
    }
    [PunRPC]
    public void L1(bool active)
    {
        stock.SmokeBras.GetComponent<PhaseChangingBras>().L1.SetActive(active);
    }
    [PunRPC]
    public void H1(bool active)
    {
        stock.SmokeBras.GetComponent<PhaseChangingBras>().H1.SetActive(active);
    }
    [PunRPC]
    public void R1(bool active)
    {
        stock.SmokeBras.GetComponent<PhaseChangingBras>().R1.SetActive(active);
    }
    [PunRPC]
    public void L2(bool active)
    {
        stock.SmokeBras.GetComponent<PhaseChangingBras>().L2.SetActive(active);
    }
    [PunRPC]
    public void R2(bool active)
    {
        stock.SmokeBras.GetComponent<PhaseChangingBras>().R2.SetActive(active);
    }
    [PunRPC]
    public void H2(bool active)
    {
        stock.SmokeBras.GetComponent<PhaseChangingBras>().H2.SetActive(active);
    }
    [PunRPC]
    public void L3(bool active)
    {
        stock.SmokeBras.GetComponent<PhaseChangingBras>().L3.SetActive(active);
    }
    [PunRPC]
    public void R3(bool active)
    {
        stock.SmokeBras.GetComponent<PhaseChangingBras>().R3.SetActive(active);
    }
    [PunRPC]
    public void H3(bool active)
    {
        stock.SmokeBras.GetComponent<PhaseChangingBras>().H3.SetActive(active);
    }
    [PunRPC]
    public void CouP2(bool active)
    {
        stock.SmokeCou.GetComponent<PhaseChanging>().P2.SetActive(active);
    }
    [PunRPC]
    public void TorseP2(bool active)
    {
        stock.SmokeTorse.GetComponent<PhaseChanging>().P2.SetActive(active);
    }
    [PunRPC]
    public void GetLight()
    {
        light = stock.mainLight.GetComponent<Light2D>();
    }
    [PunRPC]
    public void Eyes1()
    {
        eyes1 = stock.BossEyes.GetComponent<PhaseChanging>().P1.GetComponent<EyesAnim>();
    }
    [PunRPC]
    public void Eyes2()
    {
        eyes2 = stock.BossEyes.GetComponent<PhaseChanging>().P2.GetComponent<EyesAnim>();
    }
    [PunRPC]
    public void Eyes1B()
    {
        eyes1.boss = this;
    }
    [PunRPC]
    public void Eyes2B()
    {
        eyes2.boss = this;
    }
    [PunRPC]
    public void DeaD()
    {
        stock.BossComplet.GetComponent<PhaseChanging>().P3.GetComponent<Death>().boss = this;
    }
    [PunRPC]

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(step);
            stream.SendNext(phase);
            stream.SendNext(timerDps);
            stream.SendNext(wait);
            stream.SendNext(wait1);
            stream.SendNext(DPSing);
            stream.SendNext(beginning);
            stream.SendNext(maxWait);
            stream.SendNext(maxWait1);
            stream.SendNext(waiting);
            stream.SendNext(waiting1);
            stream.SendNext(endAttack);
            stream.SendNext(facingDirection);
            stream.SendNext(MaxHp);
            stream.SendNext(laserSpeed);
            stream.SendNext(damageSpeed);
            stream.SendNext(movingToLaser);
            stream.SendNext(movingFromLaser);
            stream.SendNext(timerDPSMax);
            stream.SendNext(movingToDamage);
            stream.SendNext(movingFromDamage);
            stream.SendNext(moveSpeed);
            stream.SendNext(moving);
            stream.SendNext(illuminating);
            stream.SendNext(flash);
            stream.SendNext(movingback);
            stream.SendNext(falling);
            stream.SendNext(changing);
            stream.SendNext(augmente);
            stream.SendNext(check);
            stream.SendNext(dead);
            stream.SendNext(blinding);
            stream.SendNext(aveugle);
            stream.SendNext(intensity1);

        }
        else
        {
            step = (int)stream.ReceiveNext();
            phase = (int)stream.ReceiveNext();
            timerDps = (float)stream.ReceiveNext();
            wait = (float)stream.ReceiveNext();
            wait1 = (float)stream.ReceiveNext();
            DPSing = (bool)stream.ReceiveNext();
            beginning = (bool)stream.ReceiveNext();
            maxWait = (float)stream.ReceiveNext();
            maxWait1 = (float)stream.ReceiveNext();
            waiting = (bool)stream.ReceiveNext();
            waiting1 = (bool)stream.ReceiveNext();
            endAttack = (bool)stream.ReceiveNext();
            facingDirection = (string)stream.ReceiveNext();
            MaxHp = (int)stream.ReceiveNext();
            laserSpeed = (float)stream.ReceiveNext();
            damageSpeed = (float)stream.ReceiveNext();
            movingToLaser = (bool)stream.ReceiveNext();
            movingFromLaser = (bool)stream.ReceiveNext();
            timerDPSMax = (int)stream.ReceiveNext();
            movingToDamage = (bool)stream.ReceiveNext();
            movingFromDamage = (bool)stream.ReceiveNext();
            moveSpeed = (float)stream.ReceiveNext();
            moving = (bool)stream.ReceiveNext(); ;
            illuminating = (bool)stream.ReceiveNext();
            flash = (bool)stream.ReceiveNext();
            movingback = (bool)stream.ReceiveNext();
            falling = (bool)stream.ReceiveNext();
            changing = (bool)stream.ReceiveNext();
            augmente = (bool)stream.ReceiveNext();
            check = (bool)stream.ReceiveNext();
            dead = (bool)stream.ReceiveNext();
            blinding = (bool)stream.ReceiveNext();
            aveugle = (bool)stream.ReceiveNext();
            intensity1 = (float)stream.ReceiveNext();
        }
    }
}
