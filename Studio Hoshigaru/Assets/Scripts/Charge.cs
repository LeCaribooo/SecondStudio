using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Charge : MonoBehaviour
{
    [SerializeField] PhotonView PV;
    [SerializeField] PlayerControler playerControler;
    [SerializeField] Rigidbody2D rb;
    public float dashSpeed;
    public float dashTime;
    private bool dashEnded;
    private bool canDash;
    public float dashCooldown;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] CapsuleCollider2D collider2D;
    [SerializeField] Animator anim;
    public int dmg;

    public GameObject dashEffect;
    public Transform dashEffectSpawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        dashEnded = true;
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (dashEnded && canDash)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    PhotonNetwork.Instantiate(Path.Combine("Sprites", "Player", "charge", dashEffect.name), dashEffectSpawnPoint.position, Quaternion.identity) ;
                    if (playerControler.facingRight)
                        StartCoroutine(Dash(-1f));
                    else
                        StartCoroutine(Dash(1f));
                }
            }
        }
    }

    IEnumerator Dash(float direction)
    {
        anim.SetBool("isCharging", true);
        playerControler.enabled = false;
        sprite.enabled = true;
        collider2D.enabled = true;
        dashEnded = false;
        rb.velocity = new Vector2(0f, 0f);
        rb.AddForce(Vector2.right * dashSpeed * direction, ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(dashTime);
        playerControler.enabled = true;
        collider2D.enabled = false;
        sprite.enabled = false;
        dashEnded = true;
        rb.gravityScale = gravity;
        anim.SetBool("isCharging", false);

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnEnable()
    {
        canDash = true;
        dashEnded = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponentInParent<EnemyHealth>().health -= dmg;
        }
    }


}
//Si jamais on veut faire un planneur
//rb.velocity = Vector2.left * dashSpeed;