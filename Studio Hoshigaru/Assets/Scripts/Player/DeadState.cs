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
    public GameObject myCharacter;

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
        GetMyAvatar();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            parallaxing.cam = DisplayCameraWhenDead();
        }   
    }

    IEnumerator Leave()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(4);
    }

    public Transform DisplayCameraWhenDead()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0 && !can)
        {
            StartCoroutine(RespawnCor());
            can = true;
            return myCharacter.GetComponent<PlayerControler>().camera.transform;
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

    [PunRPC]
    public void Respawn()
    {
        myCharacter.tag = "Player";
        myCharacter.GetComponent<PlayerControler>().camera.gameObject.SetActive(true);
        myCharacter.GetComponent<PlayerControler>().enabled = true;
        myCharacter.GetComponent<PlayerDeath>().isDead = false;
        myCharacter.GetComponent<Health>().numOfHits = myCharacter.GetComponent<Health>().numOfHearts * 4;
        myCharacter.GetComponent<PlayerControler>().MoveHere();
        myCharacter.GetComponent<PlayerControler>().animator.SetInteger("isDead", 2);
        Destroy(this.gameObject);
    }


    IEnumerator RespawnCor()
    {
        yield return new WaitForSeconds(3);
        base.photonView.RPC("Respawn", RpcTarget.All);
    }

    void GetMyAvatar()
    {
        GameObject[] deads = GameObject.FindGameObjectsWithTag("Dead");
        for (int i = 0; i < deads.Length; i++)
        {
            if (deads[i].GetPhotonView().Owner == PV.Owner)
                myCharacter = deads[i];
        }
    }
}
