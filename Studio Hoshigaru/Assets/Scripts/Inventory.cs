using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private bool isOpen;
    public Image avatar;
    public Image weapon;
    public GameObject weaponSelection;
    public WeaponSelection wp;
    [SerializeField] RunesManager runesManager;
    public MyUI myUI;

    [SerializeField] Sprite swordAvatar;
    [SerializeField] Sprite bowAvatar;
    [SerializeField] Sprite hammerAvatar;
    [SerializeField] Sprite shurikenAvatar;
    [SerializeField] Sprite nakedAvatar;

    //Update l'avatar en fonction de l'arme qu'il a *DONE*
    //Faire de weapon un bouton clickable, qui ouvre la fenêter de sélection des armes
    //Refaire les boutons des armes pour qu'ils deviennent des armes
    //Update l'image du bouton weapon en fonction de l'arme choisi *DONE*


    void Start()
    {
        isOpen = false;
        switch (wp.actualWeaponString)
        {
            case "hasSword":
                weapon.sprite = myUI.sword;
                avatar.sprite = swordAvatar;
                break;
            case "hasBow":
                weapon.sprite = myUI.bow;
                avatar.sprite = bowAvatar;
                break;
            case "hasShuriken":
                weapon.sprite = myUI.shuriken;
                avatar.sprite = shurikenAvatar;
                break;
            case "hasHammer":
                weapon.sprite = myUI.hammer;
                avatar.sprite = hammerAvatar;

                break;
            default:
                weapon.sprite = myUI.naked;
                avatar.sprite = nakedAvatar;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (wp.actualWeaponString)
        {
            case "hasSword":
                weapon.sprite = myUI.sword;
                avatar.sprite = swordAvatar;
                break;
            case "hasBow":
                weapon.sprite = myUI.bow;
                avatar.sprite = bowAvatar;
                break;
            case "hasShuriken":
                weapon.sprite = myUI.shuriken;
                avatar.sprite = shurikenAvatar;
                break;
            case "hasHammer":
                weapon.sprite = myUI.hammer;
                avatar.sprite = hammerAvatar;

                break;
            default:
                weapon.sprite = myUI.naked;
                avatar.sprite = nakedAvatar;
                break;
        }
    }

    public void OnClick_ShowWeaponSelection()
    {
        weaponSelection.SetActive(!isOpen);
        isOpen = !isOpen;
    }
    
    public void OnClick_CloseWeaponSelection()
    {
        runesManager.Display();
        weaponSelection.SetActive(false);
        isOpen = false;
    }
}
