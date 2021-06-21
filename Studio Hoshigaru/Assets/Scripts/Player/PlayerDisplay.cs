using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviourPun
{
    public Text name;
    public Text level;
    public Image weapon;
    public Player player;
    private WeaponSelection wp;
    private PlayerExperience pe;
    [SerializeField] private Health health;
    private Health playerHealth;

    public Sprite bow;
    public Sprite sword;
    public Sprite hammer;
    public Sprite shuriken;
    public Sprite naked;

    public GameObject affectedPlayer;

    private void Update()
    {
        level.text = pe.level.ToString();
        health.numOfHits = playerHealth.numOfHits;
        health.numOfHearts = playerHealth.numOfHearts;
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

    public void SetUp(Player _player, GameObject _affectedPlayer)
    {
        affectedPlayer = _affectedPlayer;
        player = _player;
        name.text = _player.NickName;
        playerHealth = affectedPlayer.GetComponent<Health>();
        health.numOfHits = playerHealth.numOfHits;
        health.numOfHearts = playerHealth.numOfHearts;
        wp = affectedPlayer.GetComponentInChildren<WeaponSelection>();
        pe = affectedPlayer.GetComponent<PlayerExperience>();
        level.text = pe.level.ToString();
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
