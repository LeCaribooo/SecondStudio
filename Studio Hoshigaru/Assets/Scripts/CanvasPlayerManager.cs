using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CanvasPlayerManager : MonoBehaviour
{
    public static bool isMenuOpen = false;
    public static bool isWeaponSelectionOpen = false;
    public static bool isAptitudeOpen = false;

    public GameObject weaponSelection;
    public GameObject aptitudes;
    [SerializeField] PhotonView PV;

    void Update()
    {
        isMenuOpen = isAptitudeOpen || isWeaponSelectionOpen;
       
        if(PV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isWeaponSelectionOpen)
                {
                    weaponSelection.SetActive(false);
                }
                else
                {
                    weaponSelection.SetActive(true);
                }
                isWeaponSelectionOpen = !isWeaponSelectionOpen;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isWeaponSelectionOpen)
                {
                    aptitudes.SetActive(false);
                }
                else
                {
                    aptitudes.SetActive(true);
                }
                isWeaponSelectionOpen = !isWeaponSelectionOpen;
            }

        }
    }
}
