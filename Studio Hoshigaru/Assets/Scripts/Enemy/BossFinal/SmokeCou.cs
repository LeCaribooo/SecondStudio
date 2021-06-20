using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeCou : MonoBehaviour
{
    public GameObject smoke;
    public void Grow()
    {
        transform.localScale = new Vector3(20, 20, 0);
    }

    public void End()
    {
        smoke.SetActive(false);
    }
}
