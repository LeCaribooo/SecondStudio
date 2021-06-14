using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Player_portal : MonoBehaviourPun
{
    public string nameportal;
    
    //Gestion du retour des joueurs
    [SerializeField]
    private bool WantToComeBack;
    public GameObject[] spawnpoint = new GameObject[4];
    public CopyPortal CopyPortal;

    //Nom du Level 
    public string NumberLvL;
    public string NameLvl;

    public GameObject Player;
    public Canvas Vote;
    public Canvas Level;
    public Canvas DecompteCanvas;
    public Button LevelVote;
    public Text Levelname;
    public Text text;
    public Text Count;
    public Text DecompteTxt;
    private bool HasClickE = false;
    private bool ready = false;
    public Button Ready;
    [SerializeField] Text readyUpText;
    private bool WantToTeleport = false;

    private List<bool> playerReady = new List<bool>();

    [SerializeField]
    private string[] scene = new string[1];

    private int RandomRoomNumber = 0;

    //!\ Supra méga important => Permet de savoir si c'est bien ce portail que l'on active
    public bool IsThisPortal;


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
        if (collision.gameObject.tag == "Player" &&  collision.gameObject == getMinePlayer())
        {
            IsThisPortal = true;
            Level.gameObject.SetActive(true);
            if (HasClickE)
            {
                LevelVote.interactable = false;
            }
            Levelname.text = NameLvl;
        }
        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject == getMinePlayer())
        {
            IsThisPortal = false;
            Level.gameObject.SetActive(false);
        }
    }

    public void Click_ToVote()
    {
        if (IsThisPortal)
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
    }
    private void Update()
    {
        if (HasClickE)
        {
            IsThisPortal = true;
            LevelVote.interactable = false;
            time -= Time.deltaTime;
            int sec = (int)time;
            text.text = "00 : " + sec.ToString();
            Count.text = playerReady.Count.ToString() + " / " + PhotonNetwork.PlayerList.Length.ToString();
            WantToTeleport = playerReady.Count == PhotonNetwork.PlayerList.Length;
            if (time <= 0f  || WantToTeleport) 
            {
                IsThisPortal = false;
                LevelVote.interactable = false;
                Vote.gameObject.SetActive(false);
                time = 21f;
                if (WantToTeleport)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        SelectRoom();
                    }
                    StartCoroutine(Decompte());
                }
                else
                {
                    playerReady = new List<bool>();
                    readyUpText.text = "Ready ?";
                    Ready.interactable = true;
                    LevelVote.interactable = true;
                }
                HasClickE = false;
                
            }
        }
    }


    //Teleportation
    public void LoadRandomRoom()
    {
        if (WantToComeBack)
        {
            CopyPortal.portalName = nameportal;
            CopyPortal.ComeBack = true;
            DontDestroyOnLoad(CopyPortal.gameObject);
        }
        string sceneload = scene[RandomRoomNumber];
        PhotonNetwork.LoadLevel(sceneload);
        Debug.Log("Room Loaded");

    }

    //Choix de la room
    public void SelectRoom()
    {
        foreach (string str in scene)
        {
            Debug.LogWarning(str);
        }
        int maxRandom = scene.Length;
        RandomRoomNumber = Random.Range(0, maxRandom);
        base.photonView.RPC("RandomRoomNumber_rpc", RpcTarget.All, RandomRoomNumber);
    }


    public void SendNotif()
    {
        base.photonView.RPC("SendMessage", RpcTarget.All, "Hello there");
    }

    public void VoteCanvas()
    {
        base.photonView.RPC("VoteRPC", RpcTarget.All);
    }

    public void OnClick_ReadyUp()
    {
        if (IsThisPortal)
        {
            SetReadyUp(!ready);
        }
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

    IEnumerator Decompte()
    {
        DecompteCanvas.gameObject.SetActive(true);
        DecompteTxt.text = " 3 ";
        yield return new WaitForSeconds(1f);
        DecompteTxt.text = DecompteTxt.text + " 2 ";
        yield return new WaitForSeconds(1f);
        DecompteTxt.text = DecompteTxt.text + " 1 ";
        yield return new WaitForSeconds(1f);
        DecompteTxt.text = DecompteTxt.text + "Iku !";
        yield return new WaitForSeconds(1.2f);
        DecompteCanvas.gameObject.SetActive(false);
        LoadRandomRoom();
    }
    
    [PunRPC]
    //Envoie la notif à tout le monde.
    void SendMessage(string message)
    {
        Vote.gameObject.SetActive(true);
        int nb = PhotonNetwork.PlayerList.Length;
        Debug.Log("Message envoyé :" + message);
        Debug.Log("Nombre de joueur :" + nb);
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

    [PunRPC]
    void RandomRoomNumber_rpc(int RoomNumber)
    {
        RandomRoomNumber = RoomNumber;
        Debug.Log("Room choose : " + RoomNumber);
    }
}
