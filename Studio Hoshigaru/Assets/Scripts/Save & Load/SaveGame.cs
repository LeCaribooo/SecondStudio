using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SaveGame : MonoBehaviourPun
{
    public Canvas echap;
    public Image LoadOrSave;


    private GameObject MyPlayer;

    private bool affiche = false;

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
    
    private void Start()
    {
        MyPlayer = getMinePlayer();

        /*if (MultiProfile.GetInt("load") == 1)
        {
            //Loading...
            float x = MultiProfile.GetFloat("x");
            float y = MultiProfile.GetFloat("y");
            float z = MultiProfile.GetFloat("z");
            MyPlayer.transform.position = new Vector3(x, y, z);
        }*/
    }

    public void EchapMenu()
    {
        Debug.Log("affiche : " + affiche);
        affiche = !affiche;
        if (MyPlayer == PhotonNetwork.IsMasterClient)
        {
            echap.gameObject.SetActive(affiche);
        }
        
        
    }



    public void save()
    {

        Debug.LogWarning("Saving...");
        //Game Setting
        MultiProfile.SetInt("nbplayer", PhotonNetwork.PlayerList.Length);
        for (int i = 1; i <= PhotonNetwork.PlayerList.Length; i++)
        {
            MultiProfile.SetString("player" + i , PhotonNetwork.PlayerList[i].NickName);
        }
        //Scene
        MultiProfile.SetString("scene", SceneManager.GetActiveScene().name);
        //Position
        MultiProfile.SetFloat("x", MyPlayer.transform.position.x);
        MultiProfile.SetFloat("y", MyPlayer.transform.position.y);
        MultiProfile.SetFloat("z", MyPlayer.transform.position.z);
        //Health
        MultiProfile.SetInt("nbHeart", MyPlayer.GetComponent<Health>().numOfHearts);
        MultiProfile.SetInt("numOfHits", MyPlayer.GetComponent<Health>().numOfHits);
        MultiProfile.SetInt("defense", MyPlayer.GetComponent<Health>().defense);
        MultiProfile.SetInt("regen", MyPlayer.GetComponent<Health>().regen);
        MultiProfile.SetInt("regenTime", MyPlayer.GetComponent<Health>().regenTime);
        //Player Controler
        MultiProfile.SetFloat("SpeedBoost", MyPlayer.GetComponent<PlayerControler>().speedBoost);
        MultiProfile.SetFloat("MovementSpeed", MyPlayer.GetComponent<PlayerControler>().movementSpeed);
        MultiProfile.SetFloat("JumpForce", MyPlayer.GetComponent<PlayerControler>().jumpForce);
        MultiProfile.SetInt("PlayerForce", MyPlayer.GetComponent<PlayerControler>().playerForce);
        MultiProfile.SetFloat("PlayerKnockback", MyPlayer.GetComponent<PlayerControler>().playerKnockback);
        MultiProfile.SetInt("ExtraJump", MyPlayer.GetComponent<PlayerControler>().extraJumpsValue);
        //Runes
        MultiProfile.SetInt("nbRunes", MyPlayer.GetComponent<PlayerRunes>().nbOfRunes);
        //Xp
        MultiProfile.SetInt("level", MyPlayer.GetComponent<PlayerExperience>().level);
        MultiProfile.SetInt("experience", MyPlayer.GetComponent<PlayerExperience>().experience);
        MultiProfile.SetInt("point", MyPlayer.GetComponent<PlayerExperience>().point);
        MultiProfile.SetInt("xpForNextLevel", MyPlayer.GetComponent<PlayerExperience>().expForNextLevel);

        Debug.LogWarning("... Save !");
    }
}
