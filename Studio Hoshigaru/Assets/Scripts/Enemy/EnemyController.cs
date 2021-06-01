using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Pathfinding;


public class EnemyController : MonoBehaviourPun
{
    private AIPath aIPath;
    public HealthBar healthbar;
    public EnemyHealth health;
    public EnemySO enemySO;
    private Health phealth = null;
    private PhotonView PV;
    private Animator animator;
    public bool cooling;
    public bool wait;
    public AudioSource source;

    public int maxHealth;


    // Start is called before the first frame update
    void Start()
    {
        aIPath = GetComponent<AIPath>();
        PV = GetComponent<PhotonView>();
        health.health = enemySO.health;
        healthbar.SetMaxHealth(health.health);
        animator = GetComponent<Animator>();
        cooling = true;
        source = GetComponent<AudioSource>();
        source.Play();
    }
    private void FixedUpdate()
    {
        float enemyvelocity = Mathf.Abs(aIPath.velocity.x);
        animator.SetFloat("speed", enemyvelocity);
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.SetHealth(health.health);
        Death();
        if(wait)
        {
            cooling = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Attack(other);
    }

    [PunRPC]
    public void Death()
    {
        if (health.health <= 0)
        {
            base.photonView.RPC("DestroyOnline", RpcTarget.All);
        }
    }

    public void Attack(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && cooling)
        {
            GameObject player = other.gameObject;
            phealth = player.GetComponent<Health>();
            if(PV.IsMine)
                PV.RPC("Dommage", RpcTarget.All);
            cooling = false;
            Cooler();
        }  
    }

    public void Cooler()
    {
        wait = false;
        StartCoroutine(Test());
    }
    IEnumerator Test()
    {
        yield return new WaitForSeconds(2f);
        wait = true;
    }

    [PunRPC]
    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }

    [PunRPC]
    public void Dommage()
    {
        phealth.numOfHits -= enemySO.damage;
    }
}
