using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CanvasPlayerManager : MonoBehaviour
{
    public static bool isWeaponSelectionOpen = false;
    public GameObject weaponSelection;
    [SerializeField] PhotonView PV;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PV.IsMine)
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
    }
}
