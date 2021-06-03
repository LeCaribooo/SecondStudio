using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MyUI : MonoBehaviourPun
{
    [SerializeField] Text myName;

    public Image weapon;
    public Sprite bow;
    public Sprite sword;
    public Sprite hammer;
    public Sprite shuriken;
    public Sprite naked;
    public WeaponSelection wp;


    // Start is called before the first frame update
    void Start()
    {
        myName.text = PhotonNetwork.NickName;
        switch (wp.actualWeaponString)
        {
            case "hasSword":
                weapon.sprite = sword;
                break;
            case "hasBow":
                weapon.sprite = bow;
                break;
            case "hasShuriken":
                weapon.sprite = shuriken;
                break;
            case "hasHammer":
                weapon.sprite = hammer;
                break;
            default:
                weapon.sprite = naked;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (wp.actualWeaponString)
        {
            case "hasSword":
                weapon.sprite = sword;
                break;
            case "hasBow":
                weapon.sprite = bow;
                break;
            case "hasShuriken":
                weapon.sprite = shuriken;
                break;
            case "hasHammer":
                weapon.sprite = hammer;
                break;
            default:
                weapon.sprite = naked;
                break;
        }
    }
}
