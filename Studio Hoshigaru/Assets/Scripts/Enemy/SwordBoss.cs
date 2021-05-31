using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SwordBoss : MonoBehaviourPun
{
    public int maxtimersword;
    public bool started;
    private float timersword;
    public int maxtimercharge;
    public float baseCastDist;
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
    public bool dead;
    const string LEFT = "left";
    const string RIGHT = "right";
    private GameObject[] players;
    string facingDirection;
    private bool lifended = false;
    Vector3 baseScale;
    // Start is called before the first frame update
    void Start()
    {
        facingDirection = RIGHT;
        PV = GetComponent<PhotonView>();
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

    // Update is called once per frame
    void Update()
    {
        if(started)
        {
            target = TargetChoose();
        }
        dead = Death();
        if(target != null && !dead)
        {
            if(!chargeEnded)
            {
                Charging();
            }
            if(chargeEnded && !endAttackCharge)
            {
                Wait();
            }
            if(!chargeCooling)
            {
                ChargeCooling();
            }
            if(!attackCooling)
            {
                SwordCooling();
            }
            if (endAttackCharge && endAttackSword)
            {
                if (chargeCooling)
                {
                    Charge();
                }
                else if(Vector2.Distance(transform.position, target.position) < attackDistance && attackCooling)
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
            anim.SetBool("canAttack", false);
            anim.SetBool("canWalk", false);
            anim.SetBool("canCharge", false);
        }
        else
        {
            anim.SetBool("Dead", true);
        }
        if(lifended)
        {
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
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attackBoss") && !anim.GetCurrentAnimatorStateInfo(0).IsName("deathBoss") && !anim.GetCurrentAnimatorStateInfo(0).IsName("chargeBoss"))
        {
            if (target.position.x < transform.position.x && facingDirection == RIGHT)
            {
                Flip(LEFT);
            }
            else if (target.position.x > transform.position.x && facingDirection == LEFT)
            {
                Flip(RIGHT);
            }
            if (!IsHittingWall())
            {
                anim.SetBool("canWalk", true);
                Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            }
            else
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
        }
    }
    public void Attack()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("deathBoss") && !anim.GetCurrentAnimatorStateInfo(0).IsName("chargeBoss"))
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
            anim.SetBool("canWalk", false);
            anim.SetBool("canAttack", true);
        }
    }
    public void Charge()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("deathBoss") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attackBoss"))
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
            anim.SetBool("canWalk", false);
            anim.SetBool("canCharge", true);
        }
    }

    public void Wait()
    {
        cooling -= Time.deltaTime;
        if (cooling <= 0)
        {
            endAttackCharge = true;
            cooling = maxcooling;
        }
    }

    public void Charging()
    {
        if(IsHittingWall())
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
        anim.SetBool("canCharge", false);
        chargeCooling = false;
        chargeEnded = true;
        timercharge = maxtimercharge;
        BoxCollider2D collid = hitbox.GetComponent<BoxCollider2D>();
        collid.enabled = false;
    }

    public void ChargeCooling()
    {
        timercharge -= Time.deltaTime;
        if(timercharge <= 0)
        {
            chargeCooling = true;
        }
    }

    public void SwordStopped()
    {
        anim.SetBool("canAttack", false);
        attackCooling = false;
        timersword = maxtimersword;
    }

    public void SwordCooling()
    {
        timersword -= Time.deltaTime;
        if(timersword <= 0)
        {
            attackCooling = true;
            endAttackSword = true;
        }
    }

    public bool Death()
    {
        if(health.health <= 0)
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

    [PunRPC]

    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }
}
