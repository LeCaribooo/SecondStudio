using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PlayerControler : MonoBehaviourPun, IPunObservable
{
    public PlayerSO playerSO;

    public PhotonView PV;

    public float speedBoost;
    public float movementSpeed;       //Speed du joueur
    public float jumpForce;           //Puissance de saut
    private float movementInput;       //(-1 ou 1 Gauche Droite)
    private float jumpTimeCounter;
    private float jumpTime;

    public int playerForce;
    public float playerKnockback;

    private Rigidbody2D rb;
    public Animator animator;
    private Collider2D hitbox;

    private bool isGrounded;           //Booléen pour savoir si il est sur le sol
    public Transform groundCheck;     //Coordonnées des pieds du character
    private float checkRadius;         //Radius de check
    private LayerMask whatIsGround;    //Layer qui select quel layer est le ground

    public RunesManager runesManager;

    public int extraJumpsValue;
    private int extraJumps;

    [HideInInspector] public bool facingRight = true;

    private PlayerDeath playerDeath;
    [SerializeField] private Bow bow;
    [SerializeField] private TripleShot tripleShot;

    public bool canAttack;

    public WeaponSelection ws;
    public SpriteRenderer mainSprite;
    public SpriteRenderer armlessSprite;

    public Canvas UI;
    public Canvas OtherNames;

    public Camera camera;

    private static PlayerControler playerInstance;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (playerInstance == null && PV.IsMine)
        {
            playerInstance = this;
        }
        else if (PV.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void ShootWithBow()
    {
        bow.Shoot();
    }
    
    public void TripleShootWithBow()
    {
        tripleShot.PowerShoot();
        
    }

    public void SetDeath(int i)
    {
        animator.SetInteger("isDead", i);
    }

    public void SetIsTripleShooting()
    {
        animator.SetBool("isTripleShooting", false);
    }
    public void disableForBow()
    {
        if (ws.actualWeaponString == "hasBow")
        {
            mainSprite.enabled = false;
            armlessSprite.enabled = true;
        }
        else
        {
            mainSprite.enabled = true;
            armlessSprite.enabled = false;
        }
    }


    void PlayerSO()
    {
        movementSpeed = playerSO.movementSpeed + speedBoost;
        jumpForce = playerSO.jumpForce;
        isGrounded = playerSO.isGrounded;
        checkRadius = playerSO.checkRadius;
        whatIsGround = playerSO.whatIsGround;
        extraJumpsValue = playerSO.extraJumpsValue;
        jumpTime = playerSO.jumpTime;
    }


    void Start()
    {
        //Physics2D.IgnoreLayerCollision(9, 10);
        Physics2D.IgnoreLayerCollision(9, 9);
        DontDestroyOnLoad(this.gameObject);
        canAttack = true;
        PlayerSO();
        playerDeath = GetComponent<PlayerDeath>();
        playerDeath.enabled = true;
        if (!PV.IsMine)
        {
            UI.enabled = false;
        }
        else
        {
            OtherNames.enabled = false;
        }
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        if (PV.IsMine)
        {
            Move();
            if ((facingRight && movementInput > 0) || (!facingRight && movementInput < 0))
            {
                Flip();
            }
        }
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            movementSpeed = playerSO.movementSpeed + speedBoost;
            extraJumps = extraJumpsValue;
            jumpTimeCounter = jumpTime;
            canAttack = true;
            animator.SetBool("isJumping", false);
        }
        else
        {
            canAttack = false;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
            animator.SetTrigger("takeOf");
        }

        if (Input.GetKey(KeyCode.Space) && !isGrounded && extraJumps == extraJumpsValue)
        {
            if (jumpTimeCounter > 0)
            {
                movementSpeed = 3;
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
        }
    }

    void Move()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        movementInput = Input.GetAxisRaw("Horizontal");
        if (movementInput == 0)
        {
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
        }
        rb.velocity = new Vector2(movementInput * movementSpeed, rb.velocity.y); //Déplace le rigibody
    }

    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void SetAttackStatus(int AttackStatus)
    {
        animator.SetInteger("AttackStatus", AttackStatus);
    }

    public void SetIsEstocing()
    {
        animator.SetBool("isEstocing", false);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(movementSpeed);
            stream.SendNext(jumpForce);
            stream.SendNext(extraJumpsValue);
            stream.SendNext(playerForce);
        }
        else
        {
            movementSpeed = (float)stream.ReceiveNext();
            jumpForce = (float)stream.ReceiveNext();
            extraJumpsValue = (int)stream.ReceiveNext();
            playerForce = (int)stream.ReceiveNext();
        }
    }

    public void StopHere()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isCharging", false);
        animator.SetBool("isEstocing", false);
        animator.SetBool("isTripleShooting", false);
        animator.SetInteger("AttackStatus", 0);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    public void MoveHere()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnEnable()
    {
        GameObject.Find("_GameMaster").GetComponent<Parallaxing>().cam = this.transform;
    }

}
