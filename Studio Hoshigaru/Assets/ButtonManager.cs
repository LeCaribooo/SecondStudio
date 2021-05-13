using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ButtonManager : MonoBehaviour
{

    [SerializeField] GameObject readyUp;
    [SerializeField] GameObject startGame;

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGame.SetActive(true);
        }
        else
        {
            readyUp.SetActive(true);
        }
    }
}
