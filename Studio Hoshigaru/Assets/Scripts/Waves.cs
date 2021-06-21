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
    private bool first = true;
    public bool RoomCleared = false;


    public Canvas DecompteCanvas;
    public Text DecompteTxt;
    public Canvas CanvasMob;
    public Text StatesWaves;
    public Text CWaves;

    public Portal_Back Portal_Back;

    public string oldscene;
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
            if (CountWaves > 0)
            {
                StatesWaves.text = "Vague terminée !";
            }
            DecompteCanvas.gameObject.SetActive(true);
            time -= Time.deltaTime;
            int sec = (int)time;
            DecompteTxt.text = "00 : 0" + sec.ToString();
            if (time <= 0f)
            {
                Debug.Log("Début vagues");
                DecompteCanvas.gameObject.SetActive(false);
                W_inprogress = false;
                if (PhotonNetwork.IsMasterClient)
                {
                    WavesFct();
                }    
                time = 6f;
            }
            if (PhotonNetwork.IsMasterClient)
            {
                base.photonView.RPC("SendCountMob", RpcTarget.All, nbMobs);
            }

        }
        if (!RoomCleared && !IsClear())
        {
            GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
            StatesWaves.text = "" + enemy.Length + "/" + nbMobs + " Ennemis";
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            if (player.Length == 0)
            {
                Debug.LogWarning("Bande de noob");
                RoomCleared = true;
                first = false;
                StartCoroutine(delayspawn());
            }
        }
        //Quand c'est clear et que je suis le MasterClient
        if (IsClear() && !W_inprogress && PhotonNetwork.IsMasterClient && !RoomCleared) //Permet au MasterClient de controler l'envoie de vague et leur uptade.
        {
            DecompteCanvas.gameObject.SetActive(true);
            StatesWaves.text = "Vague terminée !";
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
        if (RoomCleared && first)
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
            CWaves.text = "" + nbWaves;
            StatesWaves.text = "Salle terminée !";
            DecompteCanvas.gameObject.SetActive(false);
            Portal_Back.gameObject.SetActive(true);
            first = false;
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

    IEnumerator delayspawn()
    {
        Debug.LogWarning("Before coroutine");
        yield return new WaitForSeconds(1.05f);
        string sceneload = oldscene;
        PhotonNetwork.LoadLevel(sceneload);
        Debug.Log("Room Loaded");
    }
}
