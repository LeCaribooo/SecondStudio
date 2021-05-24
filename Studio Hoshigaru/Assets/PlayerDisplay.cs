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
    private int i;

    public Sprite bow;
    public Sprite sword;
    public Sprite hammer;
    public Sprite shuriken;
    public Sprite naked;


    public GameObject[] playersInGame;

    private void Awake()
    {
        playersInGame = GameObject.FindGameObjectsWithTag("Player");
        SetUp(player, i);
    }

    private void Update()
    { 
        playersInGame = GameObject.FindGameObjectsWithTag("Player");
        SetUp(player, i);
        level.text = pe.level.ToString();
        health.numOfHits = playerHealth.numOfHits;
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

    public void SetUp(Player _player, int i)
    {
        player = _player;
        this.i = i;
        name.text = _player.NickName;
        playerHealth = playersInGame[i].GetComponent<Health>();
        health.numOfHits = playerHealth.numOfHits;
        wp = playersInGame[i].GetComponentInChildren<WeaponSelection>();
        pe = playersInGame[i].GetComponent<PlayerExperience>();
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
