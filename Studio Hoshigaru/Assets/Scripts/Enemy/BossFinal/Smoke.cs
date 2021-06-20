using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public GameObject smoke;
    public void Grow()
    {
        transform.localScale = new Vector3(25, 25, 1);
    }

    public void End()
    {
        smoke.SetActive(false);
    }
}
