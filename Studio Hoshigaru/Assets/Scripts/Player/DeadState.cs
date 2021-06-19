﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class DeadState : MonoBehaviourPunCallbacks
{
    int actualDisplay = 0;
    Parallaxing parallaxing;
    public Canvas UI;
    private PhotonView PV;
    bool can = false;
    public GameObject myCharacter;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            parallaxing = GameObject.Find("_GameMaster").GetComponent<Parallaxing>();
            parallaxing.cam = DisplayCameraWhenDead();
            UI.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //PV.RPC("DestroyOnline", RpcTarget.All);
        if (PV.IsMine)
        {
            parallaxing.cam = DisplayCameraWhenDead();
        }   
    }

    IEnumerator Leave()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(4);
    }

    public Transform DisplayCameraWhenDead()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0 && !can)
        {
            //S'occuper de tp back;
            Respawn();
            can = true;
            return null;
        }
        else if (players.Length != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                players[actualDisplay].GetComponent<PlayerControler>().camera.gameObject.SetActive(false);
                actualDisplay++;
                actualDisplay = actualDisplay % players.Length;
            }
            players[actualDisplay].GetComponent<PlayerControler>().camera.gameObject.SetActive(true);
            return players[actualDisplay].GetComponent<PlayerControler>().camera.transform;
        }
        return null;
    }
    [PunRPC]
    void DestroyOnline()
    {
        Destroy(this.gameObject);
    }

    public void Respawn()
    {
        GameObject[] dead = GameObject.FindGameObjectsWithTag("Dead");
        for (int i = 0; i < dead.Length; i++)
        {
            dead[i].GetComponent<PlayerControler>().enabled = true;
            dead[i].GetComponent<PlayerControler>().camera.gameObject.SetActive(true);
            dead[i].GetComponent<Health>().numOfHits = dead[i].GetComponent<Health>().numOfHearts * 4;
            dead[i].GetComponent<PlayerControler>().MoveHere();
            dead[i].GetComponent<PlayerControler>().animator.SetInteger("isDead", 2);
            dead[i].tag = "Player";
        }
        base.photonView.RPC("DestroyOnline", RpcTarget.All);
    }

}
