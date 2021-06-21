﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForBosses : MonoBehaviour
{
    public string tag;
    private GameObject[] Bosses;
    public Health_Bar_Boss hb;
    public int numberOfBosses;
    // Start is called before the first frame update
    void Start()
    {
        Bosses = GameObject.FindGameObjectsWithTag("tag");
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfBosses == Bosses.Length)
        {
            hb.enabled = true;
            this.enabled = false;
        }
        else
        {
            Bosses = GameObject.FindGameObjectsWithTag("tag");
        }
    }
}
