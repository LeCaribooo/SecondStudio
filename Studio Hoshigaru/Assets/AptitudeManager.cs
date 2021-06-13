using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AptitudeManager : MonoBehaviour
{
    public PlayerExperience playerExperience;
    
    [SerializeField] private PlayerControler playerControler;
    [SerializeField] private Health phealth;
    [SerializeField] private PlayerRunes playerRunes;
    public Aptitude[] aptitudes;
    public GameObject description;
    [SerializeField] public Text text;

    public Sprite locked;
    public Sprite unlocked;
    public Sprite bougth;

    private void Start()
    {
        text = description.GetComponentInChildren<Text>();
    }

    public void EvaluateSkillTree()
    {
        for (int i = 0; i < aptitudes.Length; i++)
        {
            if (aptitudes[i].state == State.LOCKED && aptitudes[i].levelNeeded <= playerExperience.level && aptitudes[i].canBuy)
            {
                aptitudes[i].state = State.UNLOCKED;
            }
        }
    }

    public void AddHp(int heart)
    {
        phealth.numOfHearts += heart;
        phealth.numOfHits += 4 * heart;
    }

    public void AddDefense(int defense)
    {
        phealth.defense += defense;
    }

    public void AddRegen(int amount)
    {
        phealth.regen += amount;
    }

    public void AddForce(int force)
    {
        playerControler.playerForce += force;
    }

    public void AddKnockback(float knockback)
    {
        playerControler.playerKnockback += knockback;
    }

    
    public void AddSpeed(int speed)
    {
        playerControler.movementSpeed += speed;
    }

    public void AddJumpForce(int jumpForce)
    {
        playerControler.jumpForce += jumpForce;
    }

    public void AddJumps(int nbOfJumps)
    {
        playerControler.extraJumpsValue += nbOfJumps;
    }

    public void AddRunesEmplacement(int nbOfRunes)
    {
        playerRunes.nbOfRunes += nbOfRunes;
    }

}
