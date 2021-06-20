using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Golem : MonoBehaviourPun
{
    public int Maxhp;
    public int laserCooldownMax;
    public int attackCooldownMax;
    public int reinforceCooldownMax;
    public int throwCooldownMax;
    public int immuneCooldownMax;
    public int CooldownMax;
    public int phase;
    public bool LaserEnd;
    public bool ThrowingEnd;
    public bool AttackEnd;
    public bool ReinforceEnd;
    public bool ImmuneEnd;
    public bool Triggered;
    public Transform target;
    public float attackDistance;
    public float movementSpeed;
    public bool dead;
    public bool lifeEnded;
    public float baseCastDist;
    public float downCastDist;
    public float jumpForce = 7.0f;
    public float ground;
    public bool grounded;

    private EnemyHealth health;
    private Vector3 baseScale;
    private bool laserCooling;
    private bool waiting;
    private bool attackCooling;
    private bool reinforceCooling;
    private bool immuneCooling;
    public bool throwCooling;
    private float laserCooldown;
    private float cooldown;
    private float attackCooldown;
    private float throwCooldown;
    private float reinforceCooldown;
    private float immuneCooldown;
    private Rigidbody2D rb2d;
    private GameObject[] players;
    public string facingdirection;
    const string LEFT = "left";
    const string RIGHT = "right";

    public Transform downPosR;
    public Transform downPosL;
    public Transform ShockPos;
    public Transform FulguroPos;
    public Transform LaserPos;
    public Animator anim;
    public Transform castPos;
    [SerializeField] LootExperience lootExperience;

    void Start()
    {
        dead = false;
        lifeEnded = false;
        health = GetComponent<EnemyHealth>();
        health.health = Maxhp;
        laserCooldown = laserCooldownMax;
        attackCooldown = attackCooldownMax;
        reinforceCooldown = reinforceCooldownMax;
        immuneCooldown = immuneCooldownMax;
        throwCooldown = throwCooldownMax;
        downCastDist = Mathf.Abs(castPos.position.y - downPosR.position.y);
        phase = 1;
        LaserEnd = true;
        ThrowingEnd = true;
        AttackEnd = true;
        ReinforceEnd = true;
        ImmuneEnd = true;
        Triggered = false;
        rb2d = GetComponent<Rigidbody2D>();
        facingdirection = RIGHT;
        target = null;
        baseScale = transform.localScale;
        jumpForce += transform.position.y;
        ground = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Triggered)
        {
            target = TargetChoose();
        }
        dead = Death();
        if(target != null && !dead)
        {
            if(!GroundedL() && !GroundedR() && !DownWall() && !IsHittingWall())
            {
                Vector2 targetPosition = new Vector2(transform.position.x, ground);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            }
            if (attackCooling)
            {
                AttackCooling();
            }
            if(reinforceCooling)
            {
                ReinforceCooling();
            }
            if(laserCooling)
            {
                LaserCooling();
            }
            if(immuneCooling)
            {
                ImmuneCooling();
            }
            if(throwCooling)
            {
                ThrowCooling();
            }
            if(waiting)
            {
                Cooling();
            }
            if(health.health <= Maxhp / 3 * 2 && phase < 2)
            {
                waiting = true;
                phase = 2;
                anim.SetBool("HP quarter", true);
                health.health += Maxhp / 10;
            }
            if(health.health <= Maxhp / 3 && phase < 3)
            {
                waiting = true;
                phase = 3;
                anim.SetBool("HP middle", true);
                health.health += Maxhp / 10;
            }
            if(LaserEnd && AttackEnd && ImmuneEnd && ThrowingEnd && ReinforceEnd && !waiting)
            {
                if (!laserCooling)
                {
                    LaserShoot();
                }
                else if(!throwCooling && phase == 3)
                {
                    AttackDistance();
                }
                else if (!reinforceCooling && phase > 1 && Vector2.Distance(target.position,transform.position) <= attackDistance)
                {
                    Reinforce();
                }
                else if(!immuneCooling && phase != 3)
                {
                    Immune();
                }
                else if(!attackCooling && Vector2.Distance(target.position, transform.position) <= attackDistance)
                {
                    Attack();
                }
                else if(Vector2.Distance(target.position, transform.position) > attackDistance)
                {
                    if (IsHittingWall())
                    {
                        Jump();
                    }
                    else
                    {
                        Move();
                    }
                }
            }
            else if(LaserEnd && AttackEnd && ImmuneEnd && ThrowingEnd && ReinforceEnd)
            {
                Wait();
            }
        }
        else if(!dead)
        {
            anim.SetBool("StartLaser", false);
            anim.SetBool("Attack0", false);
            anim.SetBool("Block0", false);
            anim.SetBool("ArmorBuff", false);
            anim.SetBool("DistanceAttack", false);
        }
        else
        {
            anim.SetBool("Dead", true);
        }
        if(lifeEnded)
        {
            base.photonView.RPC("SendExperience", RpcTarget.All);
            base.photonView.RPC("DestroyOnline", RpcTarget.All);
            lifeEnded = false;
        }
    }

    public void Wait()
    {
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        anim.SetBool("StartLaser", false);
        anim.SetBool("Attack0", false);
        anim.SetBool("Block0", false);
        anim.SetBool("ArmorBuff", false);
        anim.SetBool("DistanceAttack", false);
    }

    public void Jump()
    {
        if (target.position.x < transform.position.x && facingdirection == RIGHT)
        {
            Flip(LEFT);
        }
        else if (target.position.x > transform.position.x && facingdirection == LEFT)
        {
            Flip(RIGHT);
        }
        else
        {
            Vector2 targetPosition = new Vector2(transform.position.x, jumpForce);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }

    public void Destroy()
    {

        lifeEnded = true;
    }

    public void LaserShoot()
    {
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Block") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Glow") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Glow 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("ArmorBuff") && !anim.GetCurrentAnimatorStateInfo(0).IsName("ArmorBuff 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attackDistance") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Throw") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 1"))
        {
            if (target.position.x < transform.position.x && facingdirection == RIGHT)
            {
                Flip(LEFT);
            }
            else if (target.position.x > transform.position.x && facingdirection == LEFT)
            {
                Flip(RIGHT);
            }
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            anim.SetBool("StartLaser", true);
            LaserEnd = false;
        }
    }
    public void Attack()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Block") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LaserCharge") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LaserCharge 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LaserCharge 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Glow") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Glow 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("ArmorBuff") && !anim.GetCurrentAnimatorStateInfo(0).IsName("ArmorBuff 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attackDistance") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
        {
            if (target.position.x < transform.position.x && facingdirection == RIGHT)
            {
                Flip(LEFT);
            }
            else if (target.position.x > transform.position.x && facingdirection == LEFT)
            {
                Flip(RIGHT);
            }
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            anim.SetBool("Attack0", true);
            AttackEnd = false;
        }
    }

    public void Attacking()
    {
        PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "shockwave"), ShockPos.position, Quaternion.identity);
    }

    public void EndAttack()
    {
        AttackEnd = true;
        attackCooling = true;
        anim.SetBool("Attack0", false);
        waiting = true;
    }

    public void Laser()     
    {
        
       PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "Laser"),LaserPos.position,Quaternion.identity);
    }

    public void LaserStop()
    {
        LaserEnd = true;
        anim.SetBool("StartLaser", false);
        waiting = true;
        laserCooling = true;
    }

    public void Reinforce()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Block") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LaserCharge") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LaserCharge 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LaserCharge 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Glow") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Glow 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attackDistance") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Throw") )
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            anim.SetBool("ArmorBuff", true);
            ReinforceEnd = false;
        }
    }
    public void EndReinforce()
    {
        anim.SetBool("ArmorBuff", false);
        ReinforceEnd = true;
        reinforceCooling = true;
        waiting = true;
    }
    public void AttackDistance()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Block") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LaserCharge") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LaserCharge 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LaserCharge 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Glow") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Glow 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Throw") && !anim.GetCurrentAnimatorStateInfo(0).IsName("ArmorBuff") && !anim.GetCurrentAnimatorStateInfo(0).IsName("ArmorBuff 1"))
        {
            if (target.position.x < transform.position.x && facingdirection == RIGHT)
            {
                Flip(LEFT);
            }
            else if (target.position.x > transform.position.x && facingdirection == LEFT)
            {
                Flip(RIGHT);
            }
            anim.SetBool("DistanceAttack", true);
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            ThrowingEnd = false;
        }
    }

    public void Throw()
    {
        GameObject newMissile = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "FulgureOPoing"), FulguroPos.position, Quaternion.identity);
    }

    public void ThrowEnd()
    {
        anim.SetBool("DistanceAttack", false);
        ThrowingEnd= true;
        throwCooling= true;
        waiting = true;
    }

    public void Immune()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LaserCharge") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LaserCharge 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LaserCharge 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Glow") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Glow 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("ArmorBuff") && !anim.GetCurrentAnimatorStateInfo(0).IsName("ArmorBuff 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Laser1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attackDistance") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            anim.SetBool("Block0", true);
            ImmuneEnd = false;
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
        }
    }
    public void EndImmune()
    {
        anim.SetBool("Block0", false);
        ImmuneEnd = true;
        immuneCooling = true;
        waiting = true;
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = true;
    }
    public void Move()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle 1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle 2"))
        {
            if (target.position.x < transform.position.x && facingdirection == RIGHT)
            {
                Flip(LEFT);
            }
            else if (target.position.x > transform.position.x && facingdirection == LEFT)
            {
                Flip(RIGHT);
            }
            else
            {
                Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            }
        }
    }

    public Transform TargetChoose()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
            return null;
        float minDist = Mathf.Infinity;
        GameObject currCloser = players[0];
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(this.transform.position, player.transform.position);
            if (distance < minDist)
            {
                currCloser = player;
                minDist = distance;
            }
        }
        return currCloser.transform;
    }

    public bool Death()
    {
        if (health.health <= 0)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ThrowCooling()
    {
        throwCooldown -= Time.deltaTime;
        if (throwCooldown <= 0)
        {
            throwCooling = false;
            throwCooldown = throwCooldownMax;
        }
    }
    public void ReinforceCooling()
    {
        reinforceCooldown -= Time.deltaTime;
        if (reinforceCooldown <= 0)
        {
            reinforceCooling = false;
            reinforceCooldown = reinforceCooldownMax;
        }
    }

    public void Cooling()
    {
        cooldown -= Time.deltaTime;
        if(cooldown <= 0)
        {
            waiting = false;
            cooldown = CooldownMax;
        }
    }

    public void LaserCooling()
    {
        laserCooldown -= Time.deltaTime;
        if (laserCooldown <= 0)
        {
            laserCooling = false;
            laserCooldown = laserCooldownMax;
        }
    }

    public void AttackCooling()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            attackCooling = false;
            attackCooldown = attackCooldownMax;
        }
    }

    public void ImmuneCooling()
    {
        immuneCooldown -= Time.deltaTime;
        if(immuneCooldown <= 0)
        {
            immuneCooling = false;
            immuneCooldown = immuneCooldownMax;
        }
    }

    public void Flip(string newDIrection)
    {
        Vector3 newScale = baseScale;

        if (newDIrection == LEFT)
        {
            newScale.x = -baseScale.x;
        }
        else
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;
        facingdirection = newDIrection;
    }
    public bool IsHittingWall()
    {
        bool val = false;
        float castDist = baseCastDist;
        //define the cast distance
        if (facingdirection == LEFT)
        {
            castDist = -baseCastDist;
        }
        //determine the target destination based on the cast distance
        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;
        Debug.DrawLine(castPos.position, targetPos, Color.green);
        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = true;
        }
        else
        {
            val = false;
        }
        return val;
    }

    public bool DownWall()
    {
        bool val = false;
        float castDist = baseCastDist;
        //define the cast distance
        if (facingdirection == LEFT)
        {
            castDist = -baseCastDist;
        }
        //determine the target destination based on the cast distance
        Vector3 targetPos = downPosR.position;
        targetPos.x += castDist;
        Debug.DrawLine(downPosR.position, targetPos, Color.green);
        if (Physics2D.Linecast(downPosR.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = true;
        }
        else
        {
            val = false;
        }
        return val;
    }
    public bool GroundedL()
    {
        bool val = true;
        float castDist = downCastDist;
        //determine the target destination based on the cast distance
        Vector3 targetPos = downPosL.position;
        targetPos.y -= castDist;
        Debug.DrawLine(downPosL.position, targetPos, Color.red);
        if (Physics2D.Linecast(downPosL.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {

            val = true;
        }
        else
        {
            val = false;
        }
        return val;
    }
    public bool GroundedR()
    {
        bool val = true;
        float castDist = downCastDist;
        //determine the target destination based on the cast distance
        Vector3 Pos = castPos.position;
        Vector3 targetPos = Pos;
        targetPos.y -= castDist;
        Debug.DrawLine(Pos, targetPos, Color.red);
        if (Physics2D.Linecast(Pos, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {

            val = true;
        }
        else
        {
            val = false;
        }
        return val;
    }
    [PunRPC]
    void SendExperience()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerExperience>().experience += lootExperience.lootedExperience;
            players[i].GetComponent<PlayerRunes>().nbOfRunes += lootExperience.runesParts;
        }
    }


    [PunRPC]

    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }
}
