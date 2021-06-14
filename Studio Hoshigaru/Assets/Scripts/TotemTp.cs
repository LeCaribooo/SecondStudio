using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TotemTp : MonoBehaviourPun
{
    public string nametotem;


    public TotemTp PairWith;

    public Text PressT;
    public Button ButtonUnlock;
    public Canvas ButtonCanvas;

    public bool HasPressT;
    private bool thisOne;
    private bool Cantp = true;
    static bool OnIt;
    static bool Unlock = false;

    GameObject getMinePlayer()
    {
        GameObject[] joueur = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < joueur.Length; i++)
        {
            if (joueur[i].GetPhotonView().IsMine)
            {
                return joueur[i];
            }
        }
        //Pas censé arrivé.
        Debug.Log("Tu n'existes pas...");
        return null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player" && collision.gameObject == getMinePlayer())
        {
            OnIt = true;
            thisOne = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject == getMinePlayer())
        {
            OnIt = false;
            thisOne = false;
        }
    }

    private void Update()
    {
        if (Unlock)
        {
            if (OnIt)
            {
                PressT.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.T) && thisOne && Cantp)
                {
                    HasPressT = true;
                }
            }
            else
            {
                PressT.gameObject.SetActive(false);
            }

            if (HasPressT && OnIt)
            {
                GameObject player = getMinePlayer();
                player.transform.position = PairWith.transform.position;
                StartCoroutine(Decompte());
                HasPressT = false;
            }
        }
        else
        {
            if (OnIt)
            {
                ButtonCanvas.gameObject.SetActive(true);
                ButtonUnlock.gameObject.SetActive(true);
            }
            else
            {
                ButtonCanvas.gameObject.SetActive(false);
                ButtonUnlock.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator Decompte()
    {
        Cantp = false;
        PairWith.Cantp = false;
        yield return new WaitForSeconds(1f);
        Cantp = true;
        PairWith.Cantp = true;
    }

    public void UnlochButton()
    {
        Unlock = true;
        PairWith.gameObject.SetActive(true);
        ButtonCanvas.gameObject.SetActive(false);
        ButtonUnlock.gameObject.SetActive(false);
    }


}
