using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class TripleShot : MonoBehaviour
{
    public Animator animator;
    public GameObject arrow;
    public float launchForce;
    public Transform shotPoint;
    public Transform crossShotTop;
    public Transform crossShotBottom;

    public AnimatedArms animatedArms;
    public float shootCooldown;
    bool canShoot = true;

    public PhotonView PV;

    void Update()
    {
        if (PV.IsMine && !CanvasPlayerManager.isMenuOpen)
        {
            if (Input.GetMouseButtonDown(1) && canShoot)
            {
                animator.Play("tripleShot");
                StartCoroutine(Wait());
            }
        }
    }

    IEnumerator Wait()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    public void PowerShoot()
    {
        if (PV.IsMine)
        {
            GameObject newArrow = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Player", arrow.name), shotPoint.position, shotPoint.rotation);
            if (animatedArms.playerControler.facingRight)
                newArrow.GetComponent<Rigidbody2D>().velocity = -transform.right * launchForce;
            else
                newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
            GameObject topArrow = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Player", arrow.name), crossShotTop.position, crossShotTop.rotation);
            if (animatedArms.playerControler.facingRight)
                topArrow.GetComponent<Rigidbody2D>().velocity = -transform.right * launchForce;
            else
                topArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
            GameObject botArrow = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Player", arrow.name), crossShotBottom.position, crossShotBottom.rotation);
            if (animatedArms.playerControler.facingRight)
                botArrow.GetComponent<Rigidbody2D>().velocity = -transform.right * launchForce;
            else
                botArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        }

    }
}
