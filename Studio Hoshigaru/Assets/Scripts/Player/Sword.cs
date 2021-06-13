using Photon.Pun;
using UnityEngine;


public class Sword : MonoBehaviour
{
    [SerializeField] private PhotonView PV;
    private int attackStatus;
    private float time;
    public BoxCollider2D attackHitboxCollider;
    public Animator animator;

    public PlayerControler pc;

    private void Start()
    {
        time = 0;
        attackStatus = 0;
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            Attack();
        }
    }

    public void Attack()
    {
        //On reset le tout 
        if (!pc.canAttack || time > 0.8 || CanvasPlayerManager.isMenuOpen) 
        {
            UpdateAttack(false,0) ;
        }
        else
        {
            //On incrémente notre timer en secondes
            if (attackStatus != 0)
            {
                time += Time.deltaTime;
            }
            //On presse le bouton d'attaque
            if(Input.GetMouseButtonDown(0))
            {
                if (attackStatus == 0 || (attackStatus == 2 && time > 0.2))
                {
                    UpdateAttack(true,1);
                }
                else if (attackStatus == 1 && time > 0.2)
                {
                    UpdateAttack(true,2);
                }
            }
        }    
    }

    private void UpdateAttack(bool update,int attackStatus)
    {
        time = 0;
        this.attackStatus = attackStatus;

        animator.SetInteger("AttackStatus", attackStatus);
    }
}
