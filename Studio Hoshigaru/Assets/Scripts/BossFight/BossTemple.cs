using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class BossTemple : MonoBehaviourPun
{


    public bool RoomCleared = false;

    public GameObject Block;

    public BossTemplePortal BossTemplePortal;

    public string newscene;




    [SerializeField]
    private GameObject[] spawnpoint = new GameObject[8];

    [SerializeField]
    private GameObject[] enemies = new GameObject[2];

    private List<GameObject> mobwaves = new List<GameObject>();

    //Timer
    float time = 6f;
    float endtime = 10f;


    //==================\\

    private void Awake()
    {
        Block.gameObject.SetActive(true);
    }

    private void Start()
    {
        Fill_mobwaves();
        int spawn = 0;
        foreach (GameObject mob in mobwaves)
        {
            Debug.Log("Spawn & Instantiate");
            int spawnPoint = spawn;
            string name = mob.name;
            Debug.Log("Mob name :" + name);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate
                    (Path.Combine("Prefab", "Enemy", name), spawnpoint[spawnPoint].transform.position, Quaternion.identity);
            }
            spawn++;


        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClearBoss() && PhotonNetwork.IsMasterClient)
        {
            RoomCleared = true;

            //Envoie des infos.
            base.photonView.RPC("SendRoomCleared", RpcTarget.Others, RoomCleared);
        }

        if (IsClearMob() && PhotonNetwork.IsMasterClient)
        {
            base.photonView.RPC("DestroyBlock", RpcTarget.Others, Block);
            Block.gameObject.SetActive(false);
        }

        if (RoomCleared)
        {
            BossTemplePortal.gameObject.SetActive(true);
        }
    }

    //Rempli la liste mobwaves de tous les monstres.
    private void Fill_mobwaves()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            int nb_mob = PhotonNetwork.PlayerList.Length;
            Debug.LogWarning("Nombre de " + enemies[i].name + " : " + nb_mob);
            for (int j = 0; j < nb_mob; j++)
            {
                mobwaves.Add(enemies[i]);
            };
        }
    }




    //Vérifie si la salle est clear de boss.
    private bool IsClearBoss()
    {
        GameObject[] boss = GameObject.FindGameObjectsWithTag("Boss");
        return boss.Length == 0;
    }

    private bool IsClearMob()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        return enemy.Length == 0;
    }


    //Envoie des info des vagues.

    [PunRPC]
    //RoomClear
    void SendRoomCleared(bool roomclear)
    {
        RoomCleared = roomclear;
    }

    [PunRPC]
    //RoomClear
    void DestroyBlock(GameObject block)
    {
        block.gameObject.SetActive(false);
    }
}
