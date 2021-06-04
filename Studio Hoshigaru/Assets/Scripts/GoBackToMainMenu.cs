using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class GoBackToMainMenu : MonoBehaviour
{
    public void OnClick_GoToMainMenu() 
    {
        SceneManager.LoadScene("PhotonLobby");
    }
}
