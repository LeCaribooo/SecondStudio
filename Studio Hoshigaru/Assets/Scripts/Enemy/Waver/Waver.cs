using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Waver : MonoBehaviourPun
{
    public HealthBar healthbar;
    public GameObject Hotzone;
    public GameObject TriggerArea;
    private Rigidbody2D rb2d;
    public GameObject hitbox;
    public Animator anim;
    private Vector3 baseScale;
    private string facingDirection;
    const string LEFT = "left";
    const string RIGHT = "right";
    public Transform castPos;
    public Transform castJump;
    public float baseCastDist;
    public float downCastDist;


    public float attackDistance;
    public int Maxhealth;
    public float movementSpeed;
    public bool attack1End;
    public bool attack2End;
    public bool attack1cooling;
    public bool attack2cooling;
    public bool cooling;
    public float attack1cooldowMax;
    public float attack2cooldownMax;
    private float attack1cooldown;
    private float attack2cooldown;
    public float cooldownMax;
    private float cooldown;

    public Transform target;
    public bool dead;
    public bool lifeEnded;
    public EnemyHealth health;

    [SerializeField] LootExperience lootExperience;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = cooldownMax;
        attack1cooldown = attack1cooldowMax;
        attack2cooldown = attack2cooldownMax;
        attack1End = true;
        attack2End = true;
        healthbar.SetMaxHealth(Maxhealth);
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
        if(!dead)
        {
            if(attack1cooling)
            {
                Attack1Cooling();
            }
            if(attack2cooling)
            {
                Attack2Cooling();
            }
            if (cooling)
            {
                Cooling();
            }
            else if(target != null)
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
                else if(attack1End && attack2End)
                {
                    if(attackDistance >= Vector2.Distance(target.position, transform.position) && !attack2cooling)
                    {
                        if (target.position.x < transform.position.x && facingDirection == RIGHT)
                        {
                            Flip(LEFT);
                        }
                        else if (target.position.x > transform.position.x && facingDirection == LEFT)
                        {
                            Flip(RIGHT);
                        }
                        Attack2();
                    }
                    else if(attackDistance >= Vector2.Distance(target.position, transform.position) && !attack1cooling)
                    {
                        if (target.position.x < transform.position.x && facingDirection == RIGHT)
                        {
                            Flip(LEFT);
                        }
                        else if (target.position.x > transform.position.x && facingDirection == LEFT)
                        {
                            Flip(RIGHT);
                        }
                        Attack1();
                    }
                    else
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
            }
            else if(attack1End && attack2End)
            {
                if(IsNearEdge() && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
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
                else if(IsHittingWall() && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
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
                else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                {

                    Move();
                }
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

    public void Attack1()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            attack1End = false;
            anim.SetBool("canWalk", false);
            anim.SetBool("canAttack1", true);
        }
    }
    public void Attack1End()
    {
        attack1End = true;
        cooling = true;
        attack1cooling = true;
        anim.SetBool("canAttack1", false);
    }
    public void Attack1Cooling()
    {
        attack1cooldown -= Time.deltaTime;
        if(attack1cooldown <= 0)
        {
            attack1cooldown = attack1cooldowMax;
            attack1cooling = false;
        }
    }

    public void Attack2()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            attack2End = false;
            anim.SetBool("canWalk", false);
            anim.SetBool("canAttack2", true);
        }
    }
    public void Attack2End()
    {
        attack2End = true;
        cooling = true;
        attack2cooling = true;
        anim.SetBool("canAttack2", false);
    }
    public void Attack2Cooling()
    {
        attack2cooldown -= Time.deltaTime;
        if (attack2cooldown <= 0)
        {
            attack2cooldown = attack1cooldowMax;
            attack2cooling = false;
        }
    }

    public void Cooling()
    {
        cooldown -= Time.deltaTime;
        if(cooldown <= 0)
        {
            cooldown = cooldownMax;
            cooling = false;
        }
    }

    public void Move()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            if (target != null)
            {
                if (!IsHittingWall())
                {
                    anim.SetBool("canWalk", true);
                    Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                }
                else
                {
                    rb2d.velocity = Vector2.up * 3f;
                }
            }
            else
            {
                anim.SetBool("canWalk", true);
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
            hitbox.SetActive(false);
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
        }
    }

    [PunRPC]

    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }
}
