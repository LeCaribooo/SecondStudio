using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.Experimental.Rendering.Universal;

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
        stock.DamageZone.GetComponent<EnemyHealth>().health = MaxHp;
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
                    stock.BossL.SetActive(false);
                    stock.BossR.SetActive(false);
                    stock.BossComplet.SetActive(true);
                    stock.BossComplet.GetComponent<PhaseChanging>().p2 = true;
                    stock.BossComplet.GetComponent<PhaseChanging>().P2.GetComponent<StateBoss>().boss = this;
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
                    stock.BossComplet.GetComponent<PhaseChanging>().P1.SetActive(false);
                    stock.BossL.SetActive(true);
                    stock.BossR.SetActive(true);
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
        Light2D light = stock.mainLight.GetComponent<Light2D>();
        if (augmente)
        {
            light.intensity += 0.01f;
        }
        else
        {
            light.intensity -= 0.01f;
        }
        if (light.intensity >= 2 && augmente)
        {
            stock.BossEyes.SetActive(false);
            stock.SmokeCou.GetComponent<PhaseChanging>().P1.GetComponent<Animator>().SetBool("Reduce", true);
            stock.SmokeTorse.GetComponent<PhaseChanging>().P1.GetComponent<Animator>().SetBool("Reduce", true);
            stock.BossComplet.SetActive(false);
            stock.SmokeBras.GetComponent<PhaseChangingBras>().P1.SetActive(false);
            stock.SmokeBras.GetComponent<PhaseChangingBras>().L1.SetActive(true);
            stock.SmokeBras.GetComponent<PhaseChangingBras>().R1.SetActive(true);
            stock.SmokeBras.GetComponent<PhaseChangingBras>().H1.SetActive(true);
            stock.SmokeH.SetActive(false);
            stock.SmokeL.SetActive(false);
            stock.SmokeR.SetActive(false);
            augmente = false;
        }
        else if (light.intensity <= 0.6f && !augmente)
        {
            aveugle = false;
            moving = true;
            augmente = true;
            light.intensity = 0.6f;
        }
    }

    public void Blind()
    {
        Light2D light = stock.mainLight.GetComponent<Light2D>();
        if (augmente)
        {
            light.intensity += 0.01f;
        }
        else
        {
            light.intensity -= 0.01f;
        }
        if (light.intensity >= 2 && augmente)
        {
            stock.BossEyes.SetActive(true);
            stock.SmokeCou.GetComponent<PhaseChanging>().P2.SetActive(true);
            stock.SmokeTorse.GetComponent<PhaseChanging>().P2.SetActive(true);
            stock.SmokeBras.GetComponent<PhaseChangingBras>().L2.SetActive(true);
            stock.SmokeBras.GetComponent<PhaseChangingBras>().R2.SetActive(true);
            stock.SmokeBras.GetComponent<PhaseChangingBras>().H2.SetActive(true);
            stock.SmokeH.SetActive(true);
            stock.SmokeL.SetActive(true);
            stock.SmokeR.SetActive(true);
            stock.DamageZone.GetComponent<EnemyHealth>().health = MaxHp;
            augmente = false;
        }

        else if (light.intensity <= 0.6f && !augmente)
        {
            blinding = false;
            light.intensity = 0.6f;
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
        stock.BossEyes.SetActive(false);
        stock.BossComplet.GetComponent<PhaseChanging>().P3.GetComponent<Death>().boss = this;
        stock.BossComplet.GetComponent<PhaseChanging>().P3.GetComponent<Animator>().SetBool("Dead", true);
    }

    public void Empty()
    {
        stock.BossComplet.SetActive(false);
        stock.BossL.SetActive(true);
        stock.BossR.SetActive(true);
        flash = true;
        Clear();
    }

    public void FlashBang()
    {
        Light2D light = stock.mainLight.GetComponent<Light2D>();
        if (augmente)
        {
            light.intensity += 0.01f;
        }
        else
        {
            light.intensity -= 0.01f;
        }
        if (light.intensity >= 5 && augmente)
        {
            stock.SmokeBras.GetComponent<PhaseChangingBras>().P2.SetActive(false);
            stock.SmokeBras.GetComponent<PhaseChangingBras>().L3.SetActive(true);
            stock.SmokeBras.GetComponent<PhaseChangingBras>().R3.SetActive(true);
            stock.SmokeBras.GetComponent<PhaseChangingBras>().H3.SetActive(true);
            stock.SmokeCou.GetComponent<PhaseChanging>().P2.GetComponent<Animator>().SetBool("Reduce", true);
            stock.SmokeTorse.GetComponent<PhaseChanging>().P2.GetComponent<Animator>().SetBool("Reduce", true);
            stock.SmokeH.SetActive(false);
            stock.SmokeL.SetActive(false);
            stock.SmokeR.SetActive(false);
            stock.BossL.GetComponent<PhaseChanging>().p1 = true;
            stock.BossR.GetComponent<PhaseChanging>().p1 = true;
            augmente = false;
        }
        else if (light.intensity <= 0.6f && !augmente)
        {
            augmente = true;
            light.intensity = 0.6f;
            flash = false;
            dead = true;
            waiting = true;
        }
    }

    public void LightPhase()
    {
        Light2D light = stock.mainLight.GetComponent<Light2D>();
        if(augmente)
        {
            light.intensity += 0.01f;
        }
        else
        {
            light.intensity -= 0.01f;
        }
        if(light.intensity >= 5 && augmente)
        {
            stock.BossL.GetComponent<PhaseChanging>().p2 = true;
            stock.BossR.GetComponent<PhaseChanging>().p2 = true;
            stock.BossSideL.GetComponent<PhaseChanging>().p2 = true;
            stock.BossSideR.GetComponent<PhaseChanging>().p2 = true;
            stock.BossEyes.GetComponent<PhaseChanging>().p2 = true;
            augmente = false;
        }
        else if(light.intensity <= 0.6f && !augmente)
        {
            augmente = true;
            light.intensity = 0.6f;
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
            EyesAnim eyes = stock.BossEyes.GetComponent<PhaseChanging>().P1.GetComponent<EyesAnim>();
            eyes.boss = this;
            eyes.phase = 1;
            eyes.anim.SetBool("Laser", true);
        }
        else
        {
            EyesAnim eyes = stock.BossEyes.GetComponent<PhaseChanging>().P2.GetComponent<EyesAnim>();
            eyes.boss = this;
            eyes.phase = 1;
            eyes.anim.SetBool("Laser", true);
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
                Debug.Log("1");
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
        else
        {
            Debug.Log("2");
            if (step == 2)
            {
                GameObject l = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserLight"), stock.Laser1.transform.position, Quaternion.identity);
                GameObject l2 = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserLight"), stock.Laser2.transform.position, Quaternion.identity);
                Laser laser = l.GetComponent<Laser>();
                Laser laser2 = l2.GetComponent<Laser>();
                laser.facingDirection = facingdirection;
                laser.boss = this;
                laser2.facingDirection = facingdirection;
                laser2.boss = this;
            }
            else if (step == 6)
            {
                Debug.Log("3");
                GameObject l = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserLight"), stock.Laser3.transform.position, Quaternion.identity);
                GameObject l2 = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserLight"), stock.Laser2.transform.position, Quaternion.identity);
                Laser laser = l.GetComponent<Laser>();
                Laser laser2 = l2.GetComponent<Laser>();
                laser.facingDirection = facingdirection;
                laser.boss = this;
                laser2.facingDirection = facingdirection;
                laser2.boss = this;
            }
            else if (step == 10)
            {
                GameObject l = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserLight"), stock.Laser1.transform.position, Quaternion.identity);
                GameObject l2 = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss", "LaserLight"), stock.Laser3.transform.position, Quaternion.identity);
                Laser laser = l.GetComponent<Laser>();
                Laser laser2 = l2.GetComponent<Laser>();
                laser.facingDirection = facingdirection;
                laser.boss = this;
                laser2.facingDirection = facingdirection;
                laser2.boss = this;
            }
        }
    }
    public void MovingHead()
    {
        if (phase == 1)
        {
            if (step == 2)
            {
                stock.BossComplet.SetActive(false);
                stock.BossSideL.SetActive(true);
                movingToLaser = true;
            }
            else if (step == 6)
            {
                stock.BossComplet.SetActive(false);
                stock.BossSideL.SetActive(true);
                movingToLaser = true;
            }
            else if (step == 10)
            {
                stock.BossComplet.SetActive(false);
                stock.BossSideR.SetActive(true);
                movingToLaser = true;
            }
        }
        else
        {
            if (step == 2 || step == 10)
            {
                stock.BossComplet.SetActive(false);
                stock.BossSideL.SetActive(true);
                movingToLaser = true;
            }
            else
            {
                stock.BossComplet.SetActive(false);
                stock.BossSideR.SetActive(true);
                movingToLaser = true;
            }
        }
    }

    public void LaserEndedL()
    {
        stock.BossSideL.SetActive(false);
        stock.BossComplet.SetActive(true);
        waiting = true;
        endAttack = true;
        step += 1;
    }

    public void LaserEndedR()
    {
        stock.BossSideR.SetActive(false);
        stock.BossComplet.SetActive(true);
        waiting = true;
        endAttack = true;
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
        else
        {
            EyesAnim eyes = stock.BossEyes.GetComponent<PhaseChanging>().P2.GetComponent<EyesAnim>();
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
        else
        {
            EyesAnim eyes = stock.BossEyes.GetComponent<PhaseChanging>().P2.GetComponent<EyesAnim>();
            eyes.boss = this;
            eyes.phase = 1;
            eyes.anim.SetBool("Tentacle", true);
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
                Warnings warn5 = stock.Tentacule1.GetComponent<Warnings>();
                Warnings warn1 = stock.Tentacule2.GetComponent<Warnings>();
                Warnings warn2 = stock.Tentacule4.GetComponent<Warnings>();
                Warnings warn3 = stock.Tentacule6.GetComponent<Warnings>();
                Warnings warn4 = stock.Tentacule7.GetComponent<Warnings>();
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
                warn1.type = 1;
                warn2.type = 1;
                warn3.type = 1;
                warn4.type = 1;
                warn5.type = 1;
                step += 1;
            }
            else if (step == 4)
            {
                Warnings warn1 = stock.Tentacule1.GetComponent<Warnings>();
                Warnings warn2 = stock.Tentacule3.GetComponent<Warnings>();
                Warnings warn3 = stock.Tentacule5.GetComponent<Warnings>();
                Warnings warn4 = stock.Tentacule4.GetComponent<Warnings>();
                Warnings warn5 = stock.Tentacule7.GetComponent<Warnings>();
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
                warn1.type = 1;
                warn2.type = 1;
                warn3.type = 1;
                warn4.type = 1;
                warn5.type = 1;
                step += 1;
            }
            else if (step == 8)
            {
                Warnings warn1 = stock.Tentacule3.GetComponent<Warnings>();
                Warnings warn2 = stock.Tentacule5.GetComponent<Warnings>();
                Warnings warn3 = stock.Tentacule6.GetComponent<Warnings>();
                Warnings warn4 = stock.Tentacule4.GetComponent<Warnings>();
                Warnings warn5 = stock.Tentacule1.GetComponent<Warnings>();
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

            Warnings warn1 = stock.Tentacule1.GetComponent<Warnings>();
            Warnings warn2 = stock.Tentacule2.GetComponent<Warnings>();
            Warnings warn3 = stock.Tentacule3.GetComponent<Warnings>();
            Warnings warn4 = stock.Tentacule4.GetComponent<Warnings>();
            Warnings warn5 = stock.Tentacule5.GetComponent<Warnings>();
            Warnings warn6 = stock.Tentacule6.GetComponent<Warnings>();
            Warnings warn7 = stock.Tentacule7.GetComponent<Warnings>();
            warn1.boss = this;
            warn2.boss = this;
            warn3.boss = this;
            warn4.boss = this;
            warn5.boss = this;
            warn6.boss = this;
            warn7.boss = this;
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
            else if (step == 9)
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
        else
        {
            if(step == 1)
            {
                Warnings warn1 = stock.Meteor1.GetComponent<Warnings>();
                Warnings warn2 = stock.Meteor2.GetComponent<Warnings>();
                Warnings warn3 = stock.Meteor3.GetComponent<Warnings>();
                Warnings warn5 = stock.Meteor5.GetComponent<Warnings>();
                Warnings warn6 = stock.Meteor6.GetComponent<Warnings>();
                Warnings warn4 = stock.Meteor8.GetComponent<Warnings>();
                warn1.boss = this;
                warn2.boss = this;
                warn3.boss = this;
                warn4.boss = this;
                warn5.boss = this;
                warn6.boss = this;
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
                Warnings warn1 = stock.Meteor1.GetComponent<Warnings>();
                Warnings warn2 = stock.Meteor7.GetComponent<Warnings>();
                Warnings warn3 = stock.Meteor3.GetComponent<Warnings>();
                Warnings warn5 = stock.Meteor4.GetComponent<Warnings>();
                Warnings warn6 = stock.Meteor6.GetComponent<Warnings>();
                Warnings warn4 = stock.Meteor8.GetComponent<Warnings>();
                warn1.boss = this;
                warn2.boss = this;
                warn3.boss = this;
                warn4.boss = this;
                warn5.boss = this;
                warn6.boss = this;
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
                Warnings warn1 = stock.Meteor1.GetComponent<Warnings>();
                Warnings warn2 = stock.Meteor2.GetComponent<Warnings>();
                Warnings warn3 = stock.Meteor4.GetComponent<Warnings>();
                Warnings warn5 = stock.Meteor5.GetComponent<Warnings>();
                Warnings warn6 = stock.Meteor7.GetComponent<Warnings>();
                Warnings warn4 = stock.Meteor8.GetComponent<Warnings>();
                warn1.boss = this;
                warn2.boss = this;
                warn3.boss = this;
                warn4.boss = this;
                warn5.boss = this;
                warn6.boss = this;
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
}
