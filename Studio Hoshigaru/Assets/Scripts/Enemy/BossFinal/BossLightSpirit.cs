using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class BossLightSpirit : MonoBehaviourPun
{
    public HealthBar healthbar;
    private Rigidbody2D rb2d;
    public Animator anim;
    private Vector3 baseScale;
    private string facingDirection;
    const string LEFT = "left";
    const string RIGHT = "right";
    public float Maxcooldown;
    private float cooldown;
    public float waitermax;
    private float wait;
    public int Maxhealth;
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
            target = TargetChoose();
            if (target.position.x < transform.position.x && facingDirection == RIGHT)
            {
                Flip(LEFT);
            }
            else if (target.position.x > transform.position.x && facingDirection == LEFT)
            {
                Flip(RIGHT);
            }
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
                if (!attackCooling)
                {

                    Attack();
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
        GameObject dark = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "TheBoss" ,"LightBullet"), dark_placement.position, Quaternion.identity);
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
    public Transform TargetChoose()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
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
