using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaitingForPlayer : MonoBehaviour
{
    private PlayerControler playerControler;
    bool check = false;
    private Transform cam;
    public Parallaxing parallaxing;

    void Update()
    {
        if(!check)
            CheckCam();
    }

    void CheckCam()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView PV = player.GetComponent<PhotonView>();
            if (PV.IsMine)
            {
                //reference de la camera
                playerControler = player.GetComponent<PlayerControler>();
                playerControler.camera.gameObject.SetActive(true);
                cam = playerControler.camera.transform;
                Camera.main.gameObject.SetActive(false);
                check = true;
                parallaxing.enabled = true;
                parallaxing.cam = cam;
            }
        }
    }
}
