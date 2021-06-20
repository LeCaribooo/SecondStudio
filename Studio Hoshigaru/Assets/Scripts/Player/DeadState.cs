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
    bool check;
    bool coroustart;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        PV = GetComponent<PhotonView>();
        GetMyAvatar();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            parallaxing = GameObject.Find("_GameMaster").GetComponent<Parallaxing>();
            parallaxing.cam = DisplayCameraWhenDead();
            UI.gameObject.SetActive(true);
        }
        base.photonView.RPC("SetTag", RpcTarget.All);
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
        if (players.Length == 0 && !coroustart)
        {
            StartCoroutine(delay());
            coroustart = true;
            return myCharacter.GetComponent<PlayerControler>().camera.transform;
        }
        else if (check)
        {
            GameObject[] deads = GameObject.FindGameObjectsWithTag("Dead");
            for (int i = 0; i < deads.Length; i++)
            {
                deads[i].GetComponent<PlayerControler>().camera.gameObject.SetActive(false);
            }
            base.photonView.RPC("Respawn", RpcTarget.All);
            check = false;
            return myCharacter.GetComponent<PlayerControler>().camera.transform;
        }
        else if (players.Length == 0)
        {
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
        myCharacter.GetComponent<PlayerControler>().enabled = true;
        myCharacter.GetComponent<PlayerDeath>().isDead = false;
        myCharacter.GetComponent<Health>().numOfHits = myCharacter.GetComponent<Health>().numOfHearts * 4;
        myCharacter.GetComponent<PlayerControler>().MoveHere();
        myCharacter.GetComponent<PlayerControler>().animator.SetInteger("isDead", 2);
        myCharacter.tag = "Player";
        Destroy(this.gameObject);
    }

    void GetMyAvatar()
    {
        GameObject[] deads = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < deads.Length; i++)
        {
            if (deads[i].GetPhotonView().Owner == PV.Owner)
            {
                Debug.Log(PV.Owner.UserId);
                myCharacter = deads[i];
                break;
            }
        }
    }

    [PunRPC]
    void SetTag()
    {
        myCharacter.tag = "Dead";
    } 

    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.25f);
        check = true;
    }
}
