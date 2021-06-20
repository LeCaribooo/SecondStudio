using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseChanging : MonoBehaviour
{
    public GameObject P1;
    public GameObject P2;
    public GameObject P3;

    public bool p1;
    public bool p2;
    public bool changed;
    public bool ended;
    // Start is called before the first frame update

    void Update()
    {
        if(p2 && !changed)
        {
            P1.SetActive(false);
            P2.SetActive(true);
            changed = true;
            p2 = false;
        }
        if(p1 && changed)
        {
            P2.SetActive(false);
            P1.SetActive(true);
            changed = true;
            p1 = false;
        }
        if(ended && changed && P3 != null)
        {
            P2.SetActive(false);
            P3.SetActive(true);
            P2 = P3;
            changed = false;
        }
    }

}
