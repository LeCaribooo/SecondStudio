using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class BossTemplePortal : MonoBehaviourPun
{
    public string nameportal;

    public BossTemple BossTemple;



    public GameObject Player;
    public Canvas Vote;
    public Canvas Level;
    public Button LevelVote;
    public Text Chronotext;
    public Text Count;
    private bool HasClickE = false;
    private bool ready = false;
    public Button Ready;
    [SerializeField] Text readyUpText;
    private bool WantToTeleport = false;

    private List<bool> playerReady = new List<bool>();



    //Timer
    float time = 21f;


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
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player" && collision.gameObject == getMinePlayer())
        {
            Level.gameObject.SetActive(true);
            if (HasClickE || WantToTeleport)
            {
                Level.gameObject.SetActive(false);
                LevelVote.gameObject.SetActive(false);
            }




        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject == getMinePlayer())
        {
            Level.gameObject.SetActive(false);
        }
    }

    public void Click_ToVote()
    {

        Level.gameObject.SetActive(false);
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach (var joueur in player)
        {
            DontDestroyOnLoad(joueur);
        }

        SendNotif();

        VoteCanvas();

    }
    private void Update()
    {
        if (HasClickE)
        {
            LevelVote.gameObject.SetActive(false);
            time -= Time.deltaTime;
            int sec = (int)time;
            Chronotext.text = "00 : " + sec.ToString();
            Count.text = playerReady.Count.ToString() + " / " + PhotonNetwork.PlayerList.Length.ToString();
            WantToTeleport = playerReady.Count == PhotonNetwork.PlayerList.Length;
            if (time <= 0f || WantToTeleport)
            {
                Level.gameObject.SetActive(false);
                LevelVote.gameObject.SetActive(false);
                Vote.gameObject.SetActive(false);
                time = 21f;
                if (WantToTeleport)
                {
                    LoadBackRoom();
                }
                else
                {
                    playerReady = new List<bool>();
                    readyUpText.text = "Ready ?";
                    Ready.interactable = true;
                    LevelVote.gameObject.SetActive(true);
                }
                HasClickE = false;

            }
        }
    }


    //Teleportation
    public void LoadBackRoom()
    {
        string sceneload = BossTemple.newscene;
        PhotonNetwork.LoadLevel(sceneload);
        Debug.Log("Room Loaded");

    }



    public void SendNotif()
    {
        base.photonView.RPC("SendMessage", RpcTarget.All);
    }

    public void VoteCanvas()
    {
        base.photonView.RPC("VoteRPC", RpcTarget.All);
    }

    public void OnClick_ReadyUp()
    {

        SetReadyUp(!ready);

    }
    private void SetReadyUp(bool state)
    {
        ready = true; //MDR cherche pas
        if (ready)
        {
            readyUpText.text = "Ready !";
            Ready.interactable = false;
            base.photonView.RPC("PlayerReady", RpcTarget.All, ready);
        }
        else
            readyUpText.text = "Ready ?";
    }


    [PunRPC]
    //Envoie la notif à tout le monde.
    void SendMessage()
    {
        Vote.gameObject.SetActive(true);
    }

    [PunRPC]
    //Attend le vote de tout le monde.
    void VoteRPC()
    {
        HasClickE = true;
    }

    [PunRPC]
    void PlayerReady(bool state)
    {
        Debug.Log("Player ready ");
        playerReady.Add(state);
    }
}
