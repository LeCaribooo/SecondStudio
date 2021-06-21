using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Levier : MonoBehaviourPun
{
    public ObjectiveManager objectiveManager;
    public GameObject[] doors;
    public SpriteRenderer spriteRenderer;
    public Sprite activatedLever;
    public Canvas pressT;
    bool OnIt;
    bool isPressed;
    GameObject onMe;
    public GameObject cameraRenderer;

    [PunRPC]
    private void Unlock()
    {
        spriteRenderer.sprite = activatedLever;
        for (int i = 0; i < doors.Length; i++)
        {
            isPressed = true;
            doors[i].GetComponent<Animator>().enabled = true;
            cameraRenderer.SetActive(false);
        }
    }

    private void Update()
    {
        if (OnIt && !isPressed)
        {
            pressT.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
            {
                base.photonView.RPC("Unlock", RpcTarget.All);   
            }
        }
        if(isPressed)
        {
            objectiveManager.NextObjective();
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
}
