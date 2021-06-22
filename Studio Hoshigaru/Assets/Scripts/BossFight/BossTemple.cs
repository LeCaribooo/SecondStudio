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

    public string oldscene;
    public string newscene;

    private bool Done = true;
    private bool BlockDestroy;
    private bool SendRoomClear;
    public bool first = false;
    public bool foursecond = false;

    [SerializeField]
    private GameObject[] spawnpoint = new GameObject[8];

    [SerializeField]
    private GameObject[] enemies = new GameObject[2];

    private List<GameObject> mobwaves = new List<GameObject>();
    public SceneBossSpawner boss;
    //Timer
    float time = 1f;


    //==================\\

    private void Awake()
    {
        first = true;
        Block.gameObject.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(foursec());
    }

    IEnumerator foursec()
    {
        yield return new WaitForSeconds(4.0f);
        RoomCleared = false;
        foursecond = true;
    }


    // Update is called once per frame
    void Update()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        if (Done)
        {
            Block.gameObject.SetActive(true);
            time -= Time.deltaTime;
            if (time <= 0f)
            {
                SpawnMob();
            }
        }
        if (IsClearBoss() && PhotonNetwork.IsMasterClient)
        {
            RoomCleared = true;

            //Envoie des infos.
            base.photonView.RPC("SendRoomCleared", RpcTarget.Others, RoomCleared);

        }

        if (IsClearMob() && PhotonNetwork.IsMasterClient && !BlockDestroy && !Done)
        {
            base.photonView.RPC("DestroyBlock", RpcTarget.Others);
            Block.gameObject.SetActive(false);
            BlockDestroy = true;
        }

        if (RoomCleared && boss.spawned && BossKilled())
        {
            //=> Raise tous les personnages morts
            if (PhotonNetwork.IsMasterClient)
            {
                GameObject[] deadplayer = GameObject.FindGameObjectsWithTag("DeadState");
                for (int i = 0; i < deadplayer.Length; i++)
                {
                    deadplayer[i].GetComponent<DeadState>().RespawnMe();
                }
                Debug.LogWarning("Respawn");
            }
            Debug.LogWarning("Portail...");
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

    private bool BossKilled()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Boss");
        return list.Length == 0;
    }

    private void SpawnMob()
    {
        int spawn = 0;
        Fill_mobwaves();
        foreach (GameObject mob in mobwaves)
        {
            Debug.Log("Spawn & Instantiate");
            int spawnpoints = spawn;
            string name = mob.name;
            Debug.Log("Mob name :" + name);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate
                    (Path.Combine("Prefab", "Enemy", name), spawnpoint[spawnpoints].transform.position, Quaternion.identity);
            }
            spawn++;
            Done = false;
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


    //RoomClear
    [PunRPC]
    void SendRoomCleared(bool roomclear)
    {
        RoomCleared = roomclear;
    }


    //Destroy Block
    [PunRPC]
    void DestroyBlock()
    {
        Block.gameObject.SetActive(false);
    }

}
