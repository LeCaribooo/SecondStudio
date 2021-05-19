using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponSelection : MonoBehaviourPun
{
    [SerializeField] private Animator animator;
    [SerializeField] public GameObject sword;
    [SerializeField] public GameObject bow;
    [SerializeField] public GameObject shuriken;
    [SerializeField] public GameObject hammer;
    public GameObject actualWeapon = null;
    public string actualWeaponString = "";


    private void Update()
    {
        switch (actualWeaponString)
        {
            case "hasSword":
                actualWeapon = sword;
                break;
            case "hasBow":
                actualWeapon = bow;
                break;
            case "hasShuriken":
                actualWeapon = shuriken;
                break;
            case "hasHammer":
                actualWeapon = hammer;
                break;
            default:
                break;
        }
    }

    public void selectSword()
    {
        if (actualWeaponString == "hasSword")
            return;
        animator.SetBool("hasSword", true);
        base.photonView.RPC("SetSwordActive", RpcTarget.All, true);
        if (actualWeaponString != "")
        {
            animator.SetBool(actualWeaponString, false);
            base.photonView.RPC("SetActualActive", RpcTarget.All, false);
        }
        base.photonView.RPC("SetActualWeapon", RpcTarget.All, "hasSword");
    }

    public void selectBow()
    {
        if (actualWeaponString == "hasBow")
            return;
        animator.SetBool("hasBow", true);
        base.photonView.RPC("SetBowActive", RpcTarget.All, true);
        if (actualWeaponString != "")
        {
            animator.SetBool(actualWeaponString, false);
            base.photonView.RPC("SetActualActive", RpcTarget.All, false);
        }
        base.photonView.RPC("SetActualWeapon", RpcTarget.All, "hasBow");
    }

    public void selectShuriken()
    {
        if (actualWeaponString == "hasShuriken")
            return;
        animator.SetBool("hasShuriken", true);
        base.photonView.RPC("SetShurikenActive", RpcTarget.All, true);
        if (actualWeaponString != "")
        {
            animator.SetBool(actualWeaponString, false);
            base.photonView.RPC("SetActualActive", RpcTarget.All, false);
        }
        base.photonView.RPC("SetActualWeapon", RpcTarget.All, "hasShuriken");
    }

    public void selectHammer()
    {
        if (actualWeaponString == "hasHammer")
            return;
        animator.SetBool("hasHammer", true);
        base.photonView.RPC("SetHammerActive", RpcTarget.All, true);
        if (actualWeaponString != "")
        {
            animator.SetBool(actualWeaponString, false);
            base.photonView.RPC("SetActualActive", RpcTarget.All, false);
        }
        base.photonView.RPC("SetActualWeapon", RpcTarget.All, "hasHammer");
    }

    [PunRPC]
    public void SetSwordActive(bool isActive)
    {
        sword.SetActive(isActive);
    }

    [PunRPC]
    public void SetBowActive(bool isActive)
    {
        bow.SetActive(isActive);
    }
    
    [PunRPC]
    public void SetShurikenActive(bool isActive)
    {
        shuriken.SetActive(isActive);
    }

    [PunRPC]
    public void SetHammerActive(bool isActive)
    {
        hammer.SetActive(isActive);
    }

    [PunRPC]
    public void SetActualActive(bool isActive)
    {
        actualWeapon.SetActive(isActive);
    }

    [PunRPC]
    public void SetActualWeapon(string actualWeaponString)
    {
        this.actualWeaponString = actualWeaponString;
    }



}


