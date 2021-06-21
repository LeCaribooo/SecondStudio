using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class DarkSpirit : MonoBehaviourPun
{
    public HealthBar healthbar;
    public GameObject Hotzone;
    public GameObject TriggerArea;
    private Rigidbody2D rb2d;
    public Animator anim;
    private Vector3 baseScale;
    private string facingDirection;
    const string LEFT = "left";
    const string RIGHT = "right";
    public Transform castPos;
    public Transform castJump;
    public float baseCastDist;
    public float downCastDist;
    public float Maxcooldown;
    private float cooldown;
    public float waitermax;
    private float wait;
    public HittingEnemy Enemy;

    public float attackDistance;
    public int Maxhealth;
    public float movementSpeed;
    public float triggeredMovementSpeed;

    public bool waiting;
    public bool attackEnd;
    public bool attackCooling;
    public Transform target;
    public Transform dark_placement;
    public bool dead;
    public bool lifeEnded;
    public EnemyHealth health;

    [SerializeField] LootExperience lootExperience;


    // Start is called before the first frame update
    void Start()
    {
        attackEnd = true;
        healthbar.SetMaxHealth(Maxhealth);
        wait = waitermax;
        cooldown = Maxcooldown;
        rb2d = GetComponent<Rigidbody2D>();
        health.health = Maxhealth;
        baseScale = transform.localScale;
        facingDirection = RIGHT;
    }

    // Update is called once per frame
    void Update()
    {
        dead = Death();
        healthbar.SetHealth(health.health);
        if (!dead)
        {
            if (attackCooling)
            {
                AttackCooling();
            }
            if (waiting || !attackEnd)
            {
                Wait();
            }
            else if (target != null)
            {
                if (target.GetComponent<Health>().numOfHits <= 0)
                {
                    Hotzone.SetActive(false);
                    TriggerArea.SetActive(true);
                    target = null;
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        anim.SetBool("canWalk", false);
                    }
                }
                else if (attackDistance >= Vector2.Distance(target.position, transform.position) && !attackCooling)
                {
                    if (target.position.x < transform.position.x && facingDirection == RIGHT)
                    {
                        Flip(LEFT);
                    }
                    else if (target.position.x > transform.position.x && facingDirection == LEFT)
                    {
                        Flip(RIGHT);
                    }
                    Attack();
                }
                else if(attackDistance < Vector2.Distance(target.position, transform.position))
                {
                    if (target.position.x < transform.position.x && facingDirection == RIGHT)
                    {
                        Flip(LEFT);
                    }
                    else if (target.position.x > transform.position.x && facingDirection == LEFT)
                    {
                        Flip(RIGHT);
                    }
                    Move();
                }
            }
            else if ((IsNearEdge() || Enemy.hittingEnemy) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Throwing"))
            {
                Enemy.hittingEnemy = false;
                if (facingDirection == LEFT)
                {
                    Flip(RIGHT);
                }
                else
                {
                    Flip(LEFT);
                }
            }
            else if (IsHittingWall() && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Throwing"))
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
            else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Throwing"))
            {
                Move();
            }
        }
        else
        {
            anim.SetBool("Death", true);
        }
        if (lifeEnded)
        {
            base.photonView.RPC("SendExperience", RpcTarget.All);
            base.photonView.RPC("DestroyOnline", RpcTarget.All);
            lifeEnded = false;
        }
    }

    public void Attack()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            attackEnd = false;
            anim.SetBool("canWalk", false);
            anim.SetBool("canAttack", true);
        }
    }
    
    public void Throw()
    {
        GameObject dark = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "DarkBullet"), dark_placement.position, Quaternion.identity);
        dark.GetComponent<DarkBullet>().facingDirection = facingDirection;
        dark.GetComponent<DarkBullet>().shootdir = (target.position - transform.position).normalized;
    }

    public void AttackEnd()
    {
        attackEnd = true;
        waiting = true;
        attackCooling = true;
        anim.SetBool("canAttack", false);
    }

    public void AttackCooling()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            cooldown = Maxcooldown;
            attackCooling = false;
        }
    }

    public void Wait()
    {
        anim.SetBool("canWalk", false);
        wait -= Time.deltaTime;
        if (wait <= 0)
        {
            wait = waitermax;
            waiting = false;
        }
    }

    public void Move()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Throwing") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            if (target != null)
            {
                if (!IsHittingWall())
                {
                    anim.SetBool("canWalk", true);
                    Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, triggeredMovementSpeed * Time.deltaTime);
                }
                else
                {
                    rb2d.velocity = Vector2.up * 3f;
                }
            }
            else
            {
                float vX = movementSpeed;
                if (facingDirection == LEFT)
                {
                    vX = -movementSpeed;
                }
                rb2d.velocity = new Vector2(vX, rb2d.velocity.y);
            }
        }
    }
    bool IsNearEdge()
    {
        bool val = true;
        float castDist = downCastDist;
        //determine the target destination based on the cast distance
        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;
        Debug.DrawLine(castPos.position, targetPos, Color.red);
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
        Debug.DrawLine(castJump.position, targetPos, Color.green);
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
        Debug.DrawLine(castPos.position, targetPos, Color.blue);
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

    public void Destroy()
    {
        lifeEnded = true;
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

    [PunRPC]
    void SendExperience()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerExperience>().experience += lootExperience.lootedExperience;
            players[i].GetComponent<PlayerRunes>().parts += lootExperience.runesParts;
        }
    }

    [PunRPC]

    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }
}
