using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseChanging : MonoBehaviour
{
    public GameObject P1;
    public GameObject P2;
    public bool p2;
    public bool changed;
    // Start is called before the first frame update

    void Update()
    {
        if(p2 && !changed)
        {
            P1.SetActive(false);
            P2.SetActive(true);
            changed = true;
        }
    }
}
