using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class Waves : MonoBehaviourPun
{
    [SerializeField]
    private int CountWaves = 1;
    
    private bool W_inprogress = true;
    
    public bool RoomCleared = false;


    public Canvas DecompteCanvas;
    public Text DecompteTxt;
    public Canvas CanvasMob;
    public Text StatesWaves;
    public Text CWaves;

    public Portal_Back Portal_Back;
    
    public string newscene;

    private int nbMobs;
    
    [SerializeField]
    private int nbWaves;

    [SerializeField]
    private int[] plusMobWaves_MAX;

    [SerializeField]
    private int[] plusMobWaves_MIN;



    [SerializeField]
    private GameObject[] spawnpoint = new GameObject[8];

    [SerializeField]
    private GameObject[] enemies = new GameObject[2];

    private List<GameObject> mobwaves = new List<GameObject>();

    //Timer
    float time = 6f;

    //== Borne Random ==\\

    // /!\ DOIT AVOIR LA MEME LENGHT QUE LE NOMBRE D'ENNEMIS DIFFERENTS
    [SerializeField]
    private int[] INIT_nbenemies_EACH_MAX = new int[2];

    // /!\ DOIT AVOIR LA MEME LENGHT QUE LE NOMBRE D'ENNEMIS DIFFERENTS
    [SerializeField]
    private int[] INIT_nbenemies_EACH_MIN = new int[2];

    //==================\\


    // Update is called once per frame
    void Update()
    {


        if (!RoomCleared)
        {
            CWaves.text = "" + (CountWaves + 1);
        }
        //Nouvelle vague
        if (CountWaves <= nbWaves && W_inprogress && !RoomCleared)
        {
            StatesWaves.text = "Wave Clear !";
            DecompteCanvas.gameObject.SetActive(true);
            time -= Time.deltaTime;
            int sec = (int)time;
            DecompteTxt.text = "00 : 0" + sec.ToString();
            if (time <= 0f)
            {
                Debug.Log("Start Waves");
                DecompteCanvas.gameObject.SetActive(false);
                W_inprogress = false;
                WavesFct();
                time = 6f;
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.LogWarning(" nombre de mob chez le MG : " + nbMobs);
                    base.photonView.RPC("SendCountMob", RpcTarget.All, nbMobs);
                }
            }

        }
        if (!RoomCleared && !IsClear())
        {
            GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
            StatesWaves.text = "" + enemy.Length + "/" + nbMobs + " Ennemies";
        }
        //Quand c'est clear et que je suis le MasterClient
        if (IsClear() && !W_inprogress && PhotonNetwork.IsMasterClient && !RoomCleared) //Permet au MasterClient de controler l'envoie de vague et leur uptade.
        {
            DecompteCanvas.gameObject.SetActive(true);
            StatesWaves.text = "Wave Clear !";
            CountWaves++;
            Debug.Log("Waves clear : " + CountWaves);
            W_inprogress = true;
            RoomCleared = CountWaves == nbWaves;

            //Envoie des infos.
            base.photonView.RPC("SendCountWaves", RpcTarget.Others, CountWaves);
            base.photonView.RPC("SendWinProgress", RpcTarget.Others, W_inprogress);
            base.photonView.RPC("SendRoomCleared", RpcTarget.Others, RoomCleared);

        }
        //Quand c'est fini
        if (RoomCleared)
        {
            CWaves.text = "" + nbWaves;
            StatesWaves.text = "Room Cleared !";
            DecompteCanvas.gameObject.SetActive(false);
            Portal_Back.gameObject.SetActive(true);
        } 
    }

    //Lance les différentes vagues.
    private void WavesFct()
    {
        ClearMobList();
        if (CountWaves > 0)
        {
            AddMob();
        }
        Fill_mobwaves();
        nbMobs = mobwaves.Count;
        foreach (GameObject mob in mobwaves)
        {
            Debug.Log("Spawn & Instantiate");
            int spawnPoint = Random.Range(0, spawnpoint.Length);
            string name = mob.name;
            Debug.Log("Mob name :" + name);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate
                    (Path.Combine("Prefab", "Enemy", name), spawnpoint[spawnPoint].transform.position, Quaternion.identity);
            }


        }
        

    }

    //Vérifie si la vague est clear.
    private bool IsClear()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        return enemy.Length == 0;
    }

    //Rempli la liste mobwaves de tous les monstres.
    private void Fill_mobwaves()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            int maxRandom = INIT_nbenemies_EACH_MAX[i];
            int minRandom = INIT_nbenemies_EACH_MIN[i];
            int nb_mob = Random.Range(minRandom, maxRandom);
            Debug.LogWarning("Nombre de " + enemies[i].name + " : " + nb_mob);
            for (int j = 0; j < nb_mob; j++)
            {
                mobwaves.Add(enemies[i]);
            }
        }
    }

    private void ClearMobList()
    {
        mobwaves = new List<GameObject>();
    }

    private void AddMob()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            int bornePLUS = plusMobWaves_MAX[i];
            int borneMIN = plusMobWaves_MIN[i];
            int Add_mob = Random.Range(borneMIN, bornePLUS);
            Debug.LogWarning("Ajout de : " + Add_mob + "Mobs");
            INIT_nbenemies_EACH_MAX[i] += Add_mob;
            INIT_nbenemies_EACH_MIN[i] += Add_mob;
        }
    }

    //Envoie des info des vagues.
    [PunRPC]
    //Countwaves
    void SendCountWaves(int countwaves)
    {
        CountWaves = countwaves;
    }

    [PunRPC]
    //Waves in progress
    void SendWinProgress(bool wInprogress)
    {
        W_inprogress = wInprogress;
    }

    [PunRPC]
    //RoomClear
    void SendRoomCleared(bool roomclear)
    {
        RoomCleared = roomclear;
    }

    [PunRPC]
    //Envoie le nb de mob
    void SendCountMob(int mobW)
    {
        nbMobs = mobW;
        Debug.LogWarning(" nombre de mobEnvoyé " + nbMobs);
    }
}
