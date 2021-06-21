using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ACDC : MonoBehaviour
{
    public DialogueUI dialogueUI;
    public DialogueObjecty dialogues;
    public Canvas pressT;
    bool OnIt;
    bool isPressed;
    GameObject onMe;
    public string ACDCname;


    private void Update()
    {
        if (OnIt && !isPressed)
        {
            pressT.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
            {
                dialogueUI.Begin(dialogues, null);
                onMe.GetComponent<Health>().numOfHits = onMe.GetComponent<Health>().numOfHearts * 4;
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
            dialogueUI.character.text = ACDCname;
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
