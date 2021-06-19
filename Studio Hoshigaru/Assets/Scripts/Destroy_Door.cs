using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Door : MonoBehaviour
{
    public static bool created = false;

    private void Awake()
    {
        if (!created)
        {
            created = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
