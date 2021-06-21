using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBoss : MonoBehaviour
{
    public MainBoss boss;

    public void Start()
    {
        boss = GameObject.FindGameObjectWithTag("BossF").GetComponent<MainBoss>();
    }

    public void P2()
    {
        boss.EndChange();
    }
}
