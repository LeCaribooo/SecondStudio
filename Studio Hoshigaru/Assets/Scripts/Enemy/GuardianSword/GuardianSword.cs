using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GuardianSword : MonoBehaviourPun
{
    public int maxtimersword;
    public bool started;
    private float timersword;
    public int maxtimercharge;
    public float baseCastDist;
    public float downCastDist;
    private float timercharge;
    public int maxcooling;
    private float cooling;
    private bool chargeEnded;
    public int dmg;
    public int maxHealth;
    public GameObject hitbox;
    
    public EnemyHealth health;
    public bool chargeCooling;
    public bool attackCooling;
    public bool endAttackSword;
    public bool endAttackCharge;
    public float attackDistance; //distance à partir de laquelle on attaque
    public float movementSpeed;//vitesse de déplacement
    public float chargeSpeed;
    public Rigidbody2D rb2d;
    private PhotonView PV;
    public Animator anim;
    public Transform target = null;
    public Transform castPos;
    public Transform castJump;
    public bool dead;
    const string LEFT = "left";
    const string RIGHT = "right";
    private GameObject[] players;
    string facingDirection;
    private bool lifended = false;
    public GameObject hotZone;
    public GameObject triggerArea;
    public HealthBar healthbar;
    Vector3 baseScale;
    [SerializeField] LootExperience lootExperience;
    // Start is called before the first frame update
    void Start()
    {
        facingDirection = RIGHT;
        PV = GetComponent<PhotonView>();
        health = GetComponent<EnemyHealth>();
        health.health = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        rb2d = GetComponent<Rigidbody2D>();
        health.health = maxHealth;
        chargeCooling = true;
        attackCooling = true;
        endAttackSword = true;
        endAttackCharge = true;
        chargeEnded = true;
        baseScale = transform.localScale;
        timercharge = maxtimercharge;
        timersword = maxtimersword;
        cooling = maxcooling;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.SetHealth(health.health);
        dead = Death();
        if (!dead && target != null && target.GetComponent<Health>().numOfHits <= 0)
        {
            hotZone.SetActive(false);
            triggerArea.SetActive(true);
            target = null;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
                anim.SetBool("CanWalk", false);
            }
        }
        if (target != null && !dead)
        {
            if (!chargeEnded)
            {
                Charging();
            }
            if (chargeEnded && !endAttackCharge)
            {
                Wait();
            }
            if (!chargeCooling)
            {
                ChargeCooling();
            }
            if (!attackCooling)
            {
                SwordCooling();
            }
            if (endAttackCharge && endAttackSword)
            {
                
                if (chargeCooling)
                {
                    Charge();
                }
                else if (Vector2.Distance(transform.position, target.position) < attackDistance && attackCooling)
                {
                    Attack();
                }
                else
                {
                    Move();
                }
            }
        }
        else if(!dead)
        {
            if (!chargeEnded)
            {
                Charging();
            }
            if (chargeEnded && !endAttackCharge)
            {
                Wait();
            }
            if (!chargeCooling)
            {
                ChargeCooling();
            }
            if (!attackCooling)
            {
                SwordCooling();
            }
            if (endAttackCharge && endAttackSword)
            {
                if (!dead && IsNearEdge() && !anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("charge"))
                {
                    if (facingDirection == LEFT)
                    {
                        Flip(RIGHT);
                    }
                    else
                    {
                        Flip(LEFT);
                    }
                }
                else if (!dead && IsHittingWall() && !anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("charge"))
                {
                    if (CanJump())
                    {
                        rb2d.velocity = Vector2.up * 3f;
                    }
                    else
                    {
                        if (facingDirection == LEFT)
                        {
                            Flip(RIGHT);
                        }
                        else
                        {
                            Flip(LEFT);
                        }
                    }
                }
                else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("charge") && !dead)
                {
                    Move();
                }
            }
        }
        else
        {
            anim.SetBool("Dead", true);
        }
        if (lifended)
        {
            base.photonView.RPC("SendExperience", RpcTarget.All);
            base.photonView.RPC("DestroyOnline", RpcTarget.All);
            lifended = false;
        }
    }

    public void Destroy()
    {

        lifended = true;
    }

    public void Move()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("charge"))
        {
            if (target != null)
            {
                if (!IsHittingWall())
                {
                    anim.SetBool("CanWalk", true);
                    Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                }
                else
                {
                    rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                }
            }
            else
            {
                anim.SetBool("CanWalk", true);
                float vX = movementSpeed;
                if (facingDirection == LEFT)
                {
                    vX = -movementSpeed;
                }
                rb2d.velocity = new Vector2(vX, rb2d.velocity.y);
            }
        }
    }
    public void Attack()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("charge"))
        {
            if (target.position.x < transform.position.x && facingDirection == RIGHT)
            {
                Flip(LEFT);
            }
            else if (target.position.x > transform.position.x && facingDirection == LEFT)
            {
                Flip(RIGHT);
            }
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            endAttackSword = false;
            anim.SetBool("CanWalk", false);
            anim.SetBool("CanAttack", true);
        }
    }

    public void Charge()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            if (target.position.x < transform.position.x && facingDirection == RIGHT)
            {
                Flip(LEFT);
            }
            else if (target.position.x > transform.position.x && facingDirection == LEFT)
            {
                Flip(RIGHT);
            }
            endAttackCharge = false;
            chargeCooling = false;
            chargeEnded = false;
            anim.SetBool("CanWalk", false);
            anim.SetBool("CanCharge", true);
        }
    }

    public void Wait()
    {
        anim.SetBool("CanWalk", false);
        anim.SetBool("CanAttack", false);
        anim.SetBool("CanCharge", false);
        cooling -= Time.deltaTime;
        if (cooling <= 0)
        {
            endAttackCharge = true;
            cooling = maxcooling;
        }
    }

    public void Charging()
    {
        if (IsHittingWall())
        {
            ChargeStopped();
        }
        else
        {
            if (facingDirection == LEFT)
            {
                transform.position += Vector3.left * chargeSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.right * chargeSpeed * Time.deltaTime;
            }
        }
    }

    bool IsHittingWall()
    {
        bool val = false;
        float castDist = baseCastDist;
        //define the cast distance
        if (facingDirection == LEFT)
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

    public void ChargeStopped()
    {
        anim.SetBool("CanCharge", false);
        chargeCooling = false;
        chargeEnded = true;
        timercharge = maxtimercharge;
        BoxCollider2D collid = hitbox.GetComponent<BoxCollider2D>();
        collid.enabled = false;
    }
    public void ChargeCooling()
    {
        timercharge -= Time.deltaTime;
        if (timercharge <= 0)
        {
            chargeCooling = true;
        }
    }

    public void SwordStopped()
    {
        anim.SetBool("CanAttack", false);
        attackCooling = false;
        timersword = maxtimersword;
    }

    public void SwordCooling()
    {
        timersword -= Time.deltaTime;
        if (timersword <= 0)
        {
            attackCooling = true;
            endAttackSword = true;
        }
    }

    public bool Death()
    {
        if (health.health <= 0)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            hitbox.SetActive(false);
            return true;
        }
        else
        {
            return false;
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
        facingDirection = newDIrection;
    }

    bool IsNearEdge()
    {
        bool val = true;
        float castDist = downCastDist;
        //determine the target destination based on the cast distance
        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;
        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {

            val = false;
        }
        else
        {
            val = true;
        }
        return val;
    }
    bool CanJump()
    {
        bool val = false;
        float castDist = baseCastDist;
        //define the cast distance
        if (facingDirection == LEFT)
        {
            castDist = -baseCastDist;
        }
        //determine the target destination based on the cast distance
        Vector3 targetPos = castJump.position;
        targetPos.x += castDist;
        Debug.DrawLine(castJump.position, targetPos, Color.red);
        if (Physics2D.Linecast(castJump.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = false;
        }
        else
        {
            val = true;
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
        }
    }


    [PunRPC]

    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }
}
