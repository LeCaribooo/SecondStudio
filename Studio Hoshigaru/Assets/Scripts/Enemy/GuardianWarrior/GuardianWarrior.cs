using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GuardianWarrior : MonoBehaviourPun
{
    public bool dead;
    private bool lifeEnded;
    public bool attack1ended = true;
    public bool attack2ended = true;
    public bool attack3ended = true;
    public bool attack1cooling = true;
    public bool attack2cooling = true;
    public bool attack3cooling = true;
    public bool cooling = true;
    public bool alreadytouched = true;

    public int Maxhealth;
    public float baseCastDist;
    public float downCastDist;
    public float maxDistanceAttack;
    public int maxtimer1;
    public int maxtimer2;
    public int maxtimer3;
    public int maxcooling1;
    public int maxcooling2;
    public int maxcooling3;
    public float movementSpeed;
    public EnemyHealth health;
    public GameObject hitbox1;
    public GameObject hitbox2;
    public GameObject hitbox3;
    public Transform castPos;
    public Transform castJump;
    public Animator anim;
    public GameObject hotZone;
    public GameObject triggerArea;
    public HealthBar healthbar;
    public HittingEnemy Enemy;

    private float timer1;
    private float timer2;
    private float timer3;
    private int last;
    public float cooling1;
    public float cooling2;
    public float cooling3;
    private Rigidbody2D rb2d;
    private Vector3 baseScale;
    private string facingDirection;
    const string LEFT = "left";
    const string RIGHT = "right";

    public Transform target;

    [SerializeField] LootExperience lootExperience;

    // Start is called before the first frame update
    void Start()
    {
        timer1 = maxtimer1;
        timer2 = maxtimer2;
        timer3 = maxtimer3;
        cooling1 = maxcooling1;
        cooling2 = maxcooling2;
        cooling3 = maxcooling3;
        health = GetComponent<EnemyHealth>();
        health.health = Maxhealth;
        rb2d = GetComponent<Rigidbody2D>();
        healthbar.SetMaxHealth(Maxhealth);
        baseScale = transform.localScale;
        facingDirection = RIGHT;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.SetHealth(health.health);
        dead = Death();
        if(!dead && target != null && target.GetComponent<Health>().numOfHits <= 0)
        {
            hotZone.SetActive(false);
            triggerArea.SetActive(true);
            target = null;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
               anim.SetBool("walk", false);
            }
        }
        if (!dead && attack1ended && attack2ended && attack3ended)
        {
            if (!attack1cooling)
            {
                Attack1Cooling();
            }
            if (!attack2cooling)
            {
                Attack2Cooling();
            }
            if (!attack3cooling)
            {
                Attack3Cooling();
            }
            if (target != null)
            {
                if (target.position.x < transform.position.x && facingDirection == RIGHT)
                {
                    Flip("left");
                }
                else if (target.position.x > transform.position.x && facingDirection == LEFT)
                {
                    Flip("right");
                }
                if (cooling)
                {
                    if (Vector2.Distance(target.position, transform.position) < maxDistanceAttack)
                    {
                        if (attack2cooling)
                        {
                            Attack2();
                            cooling = false;
                            last = 2;
                        }
                        else if (attack3cooling)
                        {
                            cooling = false;
                            Attack3();
                            last = 3;
                        }
                        else if (attack1cooling)
                        {
                            Attack1();
                            cooling = false;
                            last = 1;
                        }
                    }
                    else
                    {
                        Move();
                    }
                }
                else
                {
                    cooling = Cooling(last);
                    anim.SetBool("attack1", false);
                    anim.SetBool("attack2", false);
                    anim.SetBool("attack3", false);
                    anim.SetBool("walk", false);
                }
            }
            else if (!cooling)
            {
                cooling = Cooling(last);
                anim.SetBool("attack1", false);
                anim.SetBool("attack2", false);
                anim.SetBool("attack3", false);
                anim.SetBool("walk", false);
            }
            else if ((IsNearEdge() || Enemy.hittingEnemy) && !anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack3"))
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
            else if (IsHittingWall() && !anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack3"))
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
            else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack3"))
            {
                Move();
            }
        }
        else if (dead)
        {
            anim.SetBool("death", true);
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
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack3"))
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            attack1cooling = false;
            attack1ended = false;
            anim.SetBool("attack1", true);
            anim.SetBool("attack2", false);
            anim.SetBool("attack3", false);
            anim.SetBool("walk", false);
        }
    }

    public void Attack2()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack3"))
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            attack2cooling = false;
            attack2ended = false;
            anim.SetBool("attack1", false);
            anim.SetBool("attack2", true);
            anim.SetBool("attack3", false);
            anim.SetBool("walk", false);
        }
    }

    public void Attack3()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            attack3cooling = false;
            attack3ended = false;
            anim.SetBool("attack1", false);
            anim.SetBool("attack2", false);
            anim.SetBool("attack3", true);
            anim.SetBool("walk", false);
        }
    }

    public void Move()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack3") && !anim.GetCurrentAnimatorStateInfo(0).IsName("death"))
        {
            if (target != null)
            {
                if (!IsHittingWall())
                {
                    anim.SetBool("walk", true);
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
                float vX = movementSpeed;
                if (facingDirection == LEFT)
                {
                    vX = -movementSpeed;
                }
                rb2d.velocity = new Vector2(vX, rb2d.velocity.y);
            }
        }
    }

    public bool Cooling(int i)
    {
        bool result = false;
        switch (i)
        {
            case 1:
                cooling1 -= Time.deltaTime;
                if (cooling1 <= 0)
                {
                    result = true;
                    cooling1 = maxcooling1;
                }
                break;
            case 2:
                cooling2 -= Time.deltaTime;
                if (cooling2 <= 0)
                {
                    result = true;
                    cooling2 = maxcooling2;
                }
                break;
            case 3:
                cooling3 -= Time.deltaTime;
                if (cooling3 <= 0)
                {
                    result = true;
                    cooling3 = maxcooling3;
                }
                break;
        }
        return result;
    }

    public void Attack1Cooling()
    {
        timer1 -= Time.deltaTime;
        if (timer1 <= 0)
        {
            attack1cooling = true;
            timer1 = maxtimer1;
        }
    }

    public void Attack2Cooling()
    {
        timer2 -= Time.deltaTime;
        if (timer2 <= 0)
        {
            attack2cooling = true;
            timer2 = maxtimer2;
        }
    }

    public void Attack3Cooling()
    {
        timer3 -= Time.deltaTime;
        if (timer3 <= 0)
        {
            attack3cooling = true;
            timer3 = maxtimer3;
        }
    }

    public void Stopped1()
    {
        attack1ended = true;
        anim.SetBool("attack1", false);
        alreadytouched = true;
    }

    public void Stopped2()
    {
        attack2ended = true;
        anim.SetBool("attack2", false);
        alreadytouched = true;
    }

    public void Stopped3()
    {
        attack3ended = true;
        anim.SetBool("attack3", false);
        alreadytouched = true;
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

    public bool Death()
    {
        if (health.health <= 0)
        {
            
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            hitbox1.SetActive(false);
            hitbox2.SetActive(false);
            hitbox3.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Destroy()
    {
        lifeEnded = true;
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
