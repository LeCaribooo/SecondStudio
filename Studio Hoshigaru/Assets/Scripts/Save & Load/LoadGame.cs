using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoadGame : MonoBehaviourPun
{

    private GameObject MyPlayer;

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

    // Start is called before the first frame update
    void Start()
    {
        MyPlayer = getMinePlayer();
        //MultiProfile.SetInt("load", 0);
    }


    public void Loadgame()
    {
        Debug.LogWarning("Try to load");
        if (MultiProfile.HasKey("x"))
        {
            Debug.LogWarning("Loading...");
            MultiProfile.SetInt("load", 1);
            //Scene
            PhotonNetwork.LoadLevel(MultiProfile.GetString("scene"));
            //Position
            float x = MultiProfile.GetFloat("x");
            float y = MultiProfile.GetFloat("y");
            float z = MultiProfile.GetFloat("z");
            //Health
            int nbHeart = MultiProfile.GetInt("nbHeart");
            int numOfHits = MultiProfile.GetInt("numOfHits");
            int defense = MultiProfile.GetInt("defense");
            int regen = MultiProfile.GetInt("regen");
            int regenTime = MultiProfile.GetInt("regenTime");
            //Player Controler
            float speedboost = MultiProfile.GetFloat("SpeedBoost");
            float movementSpeed = MultiProfile.GetFloat("MovementSpeed");
            float jumpForce = MultiProfile.GetFloat("JumpForce");
            int playerForce = MultiProfile.GetInt("PlayerForce");
            float playerKnockback = MultiProfile.GetFloat("PlayerKnockback");
            int extraJump = MultiProfile.GetInt("ExtraJump");
            //Runes
            int nbRunes = MultiProfile.GetInt("nbRunes");
            //Xp
            int level = MultiProfile.GetInt("level");
            int experience = MultiProfile.GetInt("experience");
            int point = MultiProfile.GetInt("point");
            int expForNextLevel = MultiProfile.GetInt("xpForNextLevel");

            //=> Instanciation
            //Position
            MyPlayer.transform.position = new Vector3(x, y, z);
            //Health
            MyPlayer.GetComponent<Health>().numOfHearts = nbHeart;
            MyPlayer.GetComponent<Health>().numOfHits = numOfHits;
            MyPlayer.GetComponent<Health>().defense = defense;
            MyPlayer.GetComponent<Health>().regen = regen;
            MyPlayer.GetComponent<Health>().regenTime = regenTime;
            //Player Controler
            MyPlayer.GetComponent<PlayerControler>().speedBoost = speedboost;
            MyPlayer.GetComponent<PlayerControler>().movementSpeed = movementSpeed;
            MyPlayer.GetComponent<PlayerControler>().jumpForce = jumpForce;
            MyPlayer.GetComponent<PlayerControler>().playerForce= playerForce;
            MyPlayer.GetComponent<PlayerControler>().playerKnockback = playerKnockback;
            MyPlayer.GetComponent<PlayerControler>().extraJumpsValue= extraJump;
            //Runes
            MyPlayer.GetComponent<PlayerRunes>().nbOfRunes = nbRunes;
            //Xp
            MyPlayer.GetComponent<PlayerExperience>().level = level;
            MyPlayer.GetComponent<PlayerExperience>().experience = experience;
            MyPlayer.GetComponent<PlayerExperience>().point = point;
            MyPlayer.GetComponent<PlayerExperience>().expForNextLevel = expForNextLevel;

            


        }
    }
}
