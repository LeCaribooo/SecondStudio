using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class FinalFightControl : MonoBehaviourPun
{
    public string oldscene;
    public string newscene;


    public PortalFinal portal_back;

    private bool first = true;


    private void Update()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("BossF");



        if (boss.GetComponent<MainBoss>().end && first)
        {
            Debug.LogWarning("Golem mort : " + boss.GetComponent<MainBoss>().end);
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
