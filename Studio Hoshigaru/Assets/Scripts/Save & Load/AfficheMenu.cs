using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfficheMenu : MonoBehaviour
{
    private bool affiche = false;
    public Canvas echap;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            echap.GetComponent<SaveGame>().EchapMenu();
        }
    }
}
