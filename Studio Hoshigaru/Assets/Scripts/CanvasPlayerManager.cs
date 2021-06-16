using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CanvasPlayerManager : MonoBehaviour
{
    public static bool isMenuOpen = false;
    public static bool isInventoryOpen = false;
    public static bool isAptitudeOpen = false;

    public GameObject inventory;
    public GameObject aptitudes;
    [SerializeField] PhotonView PV;

    void Update()
    {
        isMenuOpen = isAptitudeOpen || isInventoryOpen;
       
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
                    isInventoryOpen = false;
                }
                isAptitudeOpen = !isAptitudeOpen;
            }
        }
    }
}
