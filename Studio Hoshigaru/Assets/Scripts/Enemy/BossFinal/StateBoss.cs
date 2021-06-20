using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBoss : MonoBehaviour
{
    public MainBoss boss;

    public void P2()
    {
        boss.EndChange();
    }
}
