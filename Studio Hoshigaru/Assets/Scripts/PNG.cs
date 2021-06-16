using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PNG : MonoBehaviourPun
{
    public Text PressT;
    //public bool HasPressT;

    private bool OnIt;

    [SerializeField] DialogueUI UI_dialogue;

    [SerializeField] DialogueObjecty dialogue;

    GameObject MyPlayer;

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

    private void Start()
    {
        MyPlayer = getMinePlayer();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player" && collision.gameObject == MyPlayer)
        {
            OnIt = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject == MyPlayer)
        {
            OnIt = false;
        }
    }

    private void Update()
    {
        if (OnIt && !UI_dialogue.isSpeaking)
        {
            PressT.gameObject.SetActive(true);
            MyPlayer.GetComponent<PlayerControler>().enabled = true;
            MyPlayer.GetComponent<PlayerControler>().MoveHere();
            MyPlayer.GetComponentInChildren<WeaponSelection>().actualWeapon.SetActive(true);

            if (Input.GetKeyDown(KeyCode.T))
            {
                MyPlayer.GetComponentInChildren<WeaponSelection>().actualWeapon.SetActive(false);
                MyPlayer.GetComponent<PlayerControler>().StopHere();
                MyPlayer.GetComponent<PlayerControler>().enabled = false;
                UI_dialogue.gameObject.SetActive(true);
                UI_dialogue.Begin(dialogue);
            }
        }
        else
        {
            PressT.gameObject.SetActive(false);
        }
    }


}
