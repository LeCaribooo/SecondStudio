using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class DeadState : MonoBehaviourPunCallbacks
{
    int actualDisplay = 0;
    Parallaxing parallaxing;
    public Canvas UI;
    private PhotonView PV;
    bool can = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            parallaxing = GameObject.Find("_GameMaster").GetComponent<Parallaxing>();
            parallaxing.cam = DisplayCameraWhenDead();
            UI.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //PV.RPC("DestroyOnline", RpcTarget.All);
        if (PV.IsMine)
        {
            parallaxing.cam = DisplayCameraWhenDead();
        }   
    }

    IEnumerator Leave()
    {
        Debug.Log("Début");
        PhotonNetwork.LeaveRoom();
        Debug.Log("PhotonNetwork.LeaveRoom");
        while (PhotonNetwork.InRoom)
            Debug.Log("On est dans la boucle");
            yield return null;
        Debug.Log("On a fini la boucle");
        SceneManager.LoadScene(4);
    }


    public override void OnLeftRoom()
    {
        Debug.Log("Il m'appelle parceque je suis un bouffon");
    }


    public Transform DisplayCameraWhenDead()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0 && !can)
        {
            StartCoroutine(Leave());
            parallaxing.enabled = false;
            can = true;
            return null;
        }
        else if (players.Length != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                players[actualDisplay].GetComponent<PlayerControler>().camera.gameObject.SetActive(false);
                actualDisplay++;
                actualDisplay = actualDisplay % players.Length;
            }
            players[actualDisplay].GetComponent<PlayerControler>().camera.gameObject.SetActive(true);
            return players[actualDisplay].GetComponent<PlayerControler>().camera.transform;
        }
        return null;
    }
}
