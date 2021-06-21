using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class BossGolemFight : MonoBehaviourPun
{
    public string oldscene;
    public string newscene;
    
    public GameObject BossGolem;

    public GameObject spawnboss;

    public PortalGolem portal_back;

    private bool first = true;


    void Start()
    {
        StartCoroutine(justASecond());
    }

    IEnumerator justASecond()
    {
        yield return new WaitForSeconds(2.0f);
        if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.Instantiate(Path.Combine("Prefab", "Enemy", "GolemBoss"), spawnboss.transform.position, Quaternion.identity);
        
    }

    private void Update()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        if (player.Length == 0 && first)
        {
            Debug.LogWarning("Bande de noob");
            first = false;
            StartCoroutine(delayspawn());
        }

        
        if (boss.GetComponent<Golem>().deade && first)
        {
            Debug.LogWarning("Golem mort : " + boss.GetComponent<Golem>().deade);
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
            first = false;
            portal_back.gameObject.SetActive(true);
        }

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
