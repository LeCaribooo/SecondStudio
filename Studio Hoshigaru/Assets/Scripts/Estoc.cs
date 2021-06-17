using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Estoc : MonoBehaviourPun
{

    [SerializeField] PlayerControler playerControler;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PhotonView PV;
    public float dashSpeed;
    public float dashTime;
    private bool dashEnded;
    private bool canDash;
    public float dashCooldown;
    [SerializeField] Animator anim;
    public int dmg;

    float gravity;

    private int estocState;
    public GameObject dashEffect;
    public Transform dashEffectSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        dashEnded = true;
        canDash = true;
        gravity = rb.gravityScale;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (dashEnded && canDash && estocState == 0)
                {
                    estocState++;
                    PhotonNetwork.Instantiate(Path.Combine("Sprites","Player","charge",dashEffect.name), dashEffectSpawnPoint.position, Quaternion.identity);
                    if (playerControler.facingRight)
                        StartCoroutine(EstocDash(-1f));
                    else
                        StartCoroutine(EstocDash(1f));
                }
                else if (estocState == 1)
                {
                    anim.SetBool("isEstocing", false);
                    anim.SetBool("isCharging", false);
                    StopAllCoroutines();
                    
                    StartCoroutine(Cooldown());
                    estocState = 0;
                    dashEnded = true;
                    playerControler.enabled = true;
                    rb.gravityScale = gravity;
                }
            }
        }
    }

    IEnumerator EstocDash(float direction)
    {
        anim.SetBool("isCharging", true);
        playerControler.enabled = false;
        dashEnded = false;
        rb.velocity = new Vector2(0f, 0f);
        rb.AddForce(Vector2.right * dashSpeed * direction, ForceMode2D.Impulse);
        rb.gravityScale = 0;
        
        yield return new WaitForSeconds(dashTime);
        anim.SetBool("isCharging", false);
        anim.SetBool("isEstocing", true);
        
        dashEnded = true;
        playerControler.enabled = true;
        rb.gravityScale = gravity;
        estocState = 0;
        
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
        dashEnded = true;
        canDash = true;
    }
}
