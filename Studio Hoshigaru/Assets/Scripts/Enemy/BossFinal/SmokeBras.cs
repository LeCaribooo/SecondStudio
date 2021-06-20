using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBras : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject smoke;
    public GameObject Growed;
    public void Grow()
    {
        smoke.SetActive(false);
        if (Growed != null)
        {
            Growed.SetActive(true);
        }
    }

    public void End()
    {
        smoke.SetActive(false);
    }
}
