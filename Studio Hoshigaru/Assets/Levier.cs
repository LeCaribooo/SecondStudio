﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Levier : MonoBehaviourPun
{
    public GameObject[] doors;
    public SpriteRenderer spriteRenderer;
    public Sprite activatedLever;
    public Canvas pressT;
    bool OnIt;
    bool isPressed;
    GameObject onMe;

    [PunRPC]
    private void Unlock()
    {
        spriteRenderer.sprite = activatedLever;
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].GetComponent<Animator>().enabled = true;
        }
    }

    private void Update()
    {
        if (OnIt)
        {
            pressT.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
            {
                base.photonView.RPC("Unlock", RpcTarget.All);
                isPressed = true;
            }
        }
        if(isPressed)
        {
            pressT.gameObject.SetActive(false);
            this.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onMe = collision.gameObject;
            OnIt = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onMe = null;
            OnIt = false;
            pressT.gameObject.SetActive(false);
        } 
    }
}
