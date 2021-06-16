using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DataRoom : MonoBehaviourPun
{
    public static string sceneToLoad;
    public static int nbplayers;
    


    public void fillData()
    {
        sceneToLoad = MultiProfile.GetString("scene");
        nbplayers = MultiProfile.GetInt("nbplayer");
    }
    
}
