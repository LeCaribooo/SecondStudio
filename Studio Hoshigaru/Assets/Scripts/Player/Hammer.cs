using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Hammer : MonoBehaviour
{
    [SerializeField] BoxCollider2D hitbox;
    public Animator animator;
    public PhotonView PV;
    public PlayerControler pc;

    private void Start()
    {
        hitbox.enabled = false;
    }

    void Update()
    {
        if (PV.IsMine)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && pc.canAttack && !CanvasPlayerManager.isWeaponSelectionOpen)
        {
            animator.SetInteger("AttackStatus", 1);
        }
    }
}
