using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Chest : MonoBehaviourPun
{
    public Canvas pressT;
    public string function;
    bool OnIt;
    bool isPressed;
    GameObject onMe;
    public GameObject cameraRenderer;
    public PhotonView PV;

    private void Update()
    {
        if (OnIt)
        {
            pressT.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
            {
                base.photonView.RPC("DesactiveCameraRenderer", RpcTarget.All);
                onMe.GetComponentInChildren<WeaponSelection>().OnClick_ActiveButton(function);
                isPressed = true;
            }
        }
        if (isPressed)
        {
            pressT.gameObject.SetActive(false);
            this.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject.GetPhotonView().IsMine)
        {
            onMe = collision.gameObject;
            OnIt = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject.GetPhotonView().IsMine)
        {
            onMe = null;
            OnIt = false;
            pressT.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void DesactiveCameraRenderer()
    {
        if(cameraRenderer != null)
        {
            cameraRenderer.SetActive(false);
        }
    }
}
