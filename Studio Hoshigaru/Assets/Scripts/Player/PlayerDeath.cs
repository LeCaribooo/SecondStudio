using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.SceneManagement;
public class PlayerDeath : MonoBehaviourPun
{
    private Health health;
    private PhotonView PV;
    public Animator animator;
    public PlayerControler playerControler;
    public CapsuleCollider2D capsuleCollider;
    public Rigidbody2D rb;
    public Camera camera;
    bool isDead = false;
   
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (health.numOfHits <= 0 && !isDead)
            {
                isDead = true;
                base.photonView.RPC("Unload", RpcTarget.All);
            }
        }

        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            Destroy(this.gameObject);
        }
    }

    public void Death()
    {
        camera.gameObject.SetActive(false);
        PhotonNetwork.Instantiate(Path.Combine("Prefab", "Player", "DeadPlayer"), transform.position, Quaternion.identity, 0); ;
    }

    [PunRPC]
   void Unload()
    {
        animator.SetTrigger("isDead");
        playerControler.enabled = false;
        gameObject.tag = "Untagged";
   }

}
