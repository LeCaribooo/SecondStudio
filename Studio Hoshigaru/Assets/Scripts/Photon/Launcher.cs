using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;


    [SerializeField] InputField roomNameInputField;
    [SerializeField] Text errorText;
    [SerializeField] Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] Text readyUpText;
    [SerializeField] Text playerNameText;
    [SerializeField] Button startButton;

    private List<PlayerListItem> playerList = new List<PlayerListItem>();

    private bool ready = false;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("Connecting to Master");
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        else
        {
            MenuManager.Instance.OpenMenu("title");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if (PhotonNetwork.NickName == "")
        {
            MenuManager.Instance.OpenMenu("main");
            Debug.Log("Joined Lobby");
        }
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOps);

        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform item in playerListContent)
        {
            Destroy(item.gameObject);
        }
        playerList.Clear();

        for (int i = 0; i < players.Count(); i++)
        {
            GameObject player = Instantiate(playerListPrefab, playerListContent);
            player.GetComponent<PlayerListItem>().SetUp(players[i]);
            playerList.Add(player.GetComponent<PlayerListItem>());
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed" + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
        ready = false;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform item in roomListContent)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject player = Instantiate(playerListPrefab, playerListContent);
        player.GetComponent<PlayerListItem>().SetUp(newPlayer);
        playerList.Add(player.GetComponent<PlayerListItem>());
    }

    private void SetReadyUp(bool state)
    {
        ready = state;
        if (ready)
            readyUpText.text = "Prêt";
        else
            readyUpText.text = "Pas prêt";
    }

    public void OnClick_StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < playerList.Count(); i++)
            {
                if (!playerList[i].Ready)
                   return;
            }
            startButton.interactable = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void OnClick_ReadyUp()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            SetReadyUp(!ready);
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.All, PhotonNetwork.LocalPlayer, ready);
        }
    }

    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = playerList.FindIndex(x => x.player == player);
        if (index != -1)
        {
            playerList[index].Ready = ready;
            if(ready)
                playerList[index].text.color = Color.green;
            else
                playerList[index].text.color = Color.white;
        }  
    }

    public void OnClick_ValidButton()
    {
        PhotonNetwork.NickName = playerNameText.text;
    }


   public void OnClick_QuitApplication()
    {
        Application.Quit();
    }

}
