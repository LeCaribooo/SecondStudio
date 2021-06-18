using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CanvasPlayerManager : MonoBehaviour
{
    public static bool isMenuOpen = false;
    public static bool isInventoryOpen = false;
    public static bool isAptitudeOpen = false;
    public static bool isEscapeOpen = false;
    

    public GameObject inventory;
    public GameObject aptitudes;
    public GameObject escape;
    [SerializeField] PhotonView PV;

    void Update()
    {
        isMenuOpen = isAptitudeOpen || isInventoryOpen || isEscapeOpen;
       
        if(PV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isInventoryOpen)
                {
                    inventory.SetActive(false);
                }
                else
                {
                    inventory.SetActive(true);
                    aptitudes.SetActive(false);
                    isAptitudeOpen = false;
                    escape.SetActive(false);
                    isEscapeOpen = false;
                }
                isInventoryOpen = !isInventoryOpen;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isAptitudeOpen)
                {
                    aptitudes.SetActive(false);
                }
                else
                {
                    aptitudes.SetActive(true);
                    inventory.SetActive(false);
                    escape.SetActive(false);
                    isEscapeOpen = false;
                    isInventoryOpen = false;
                }
                isAptitudeOpen = !isAptitudeOpen;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isEscapeOpen)
                {
                    escape.SetActive(false);
                }
                else
                {
                    escape.SetActive(true);
                    aptitudes.SetActive(false);
                    inventory.SetActive(false);
                    isInventoryOpen = false;
                    isAptitudeOpen = false;
                }
                isEscapeOpen = !isEscapeOpen;
            }
        }
    }
}
