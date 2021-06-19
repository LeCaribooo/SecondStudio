    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ena : MonoBehaviour
{
    public DialogueUI dialogueUI;
    public List<DialogueObjecty> dialoguesEna = new List<DialogueObjecty>();
    private bool hasPlayedExperiencDialogue = false;
    private bool hasPlayedInventoryDialogue = false;
    private bool hasPlayedBossesDialogue = false;
    public Chest chest;

    GameObject MyPlayer;

    GameObject getMinePlayer()
    {
        GameObject[] joueur = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < joueur.Length; i++)
        {
            if (joueur[i].GetPhotonView().IsMine)
            {
                return joueur[i];
            }
        }
        //Pas censé arrivé.
        Debug.Log("Tu n'existes pas...");
        return null;
    }


    public void Awake()
    {
        if (!hasPlayedBossesDialogue && Destroy_Door.created)
            PlayBossesDialogue();
    }


    // Start is called before the first frame update
    void Start()
    {
        MyPlayer = getMinePlayer();
        dialogueUI.character.text = "Ena";
        dialogueUI.Begin(dialoguesEna[0], null);    
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasPlayedExperiencDialogue && MyPlayer.GetComponent<PlayerExperience>().level == 2)
        {
            PlayExperienceDialogue();
        }

        if(!hasPlayedInventoryDialogue && !chest.isActiveAndEnabled)
        {
            PlayInventoryDialogue();
        }

        if (dialogueUI.isSpeaking)
        {
            MyPlayer.GetComponent<PlayerControler>().StopHere();
            MyPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            MyPlayer.GetComponent<PlayerControler>().enabled = false;
            if(MyPlayer.GetComponentInChildren<WeaponSelection>().actualWeapon != null)
                MyPlayer.GetComponentInChildren<WeaponSelection>().actualWeapon.SetActive(false);
        }
        else
        {
            MyPlayer.GetComponent<PlayerControler>().enabled = true;
            MyPlayer.GetComponent<PlayerControler>().MoveHere();
            if (MyPlayer.GetComponentInChildren<WeaponSelection>().actualWeapon != null)
                MyPlayer.GetComponentInChildren<WeaponSelection>().actualWeapon.SetActive(true);
        }
    }


    void PlayExperienceDialogue()
    {
        MyPlayer.GetComponent<Health>().isInvisible = true;
        dialogueUI.Begin(dialoguesEna[2], null);
        hasPlayedExperiencDialogue = true;
        StartCoroutine(Wait());
    }


    void PlayInventoryDialogue()
    {
        dialogueUI.Begin(dialoguesEna[1], null);
        hasPlayedInventoryDialogue = true;
    }

    void PlayBossesDialogue()
    {
        dialogueUI.Begin(dialoguesEna[3], null);
        hasPlayedBossesDialogue = true;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(6);
        MyPlayer.GetComponent<Health>().isInvisible = false;
    }
}
