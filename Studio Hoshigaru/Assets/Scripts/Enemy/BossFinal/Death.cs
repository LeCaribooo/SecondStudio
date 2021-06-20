using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    // Start is called before the first frame update
    public MainBoss boss;

    public void FinalDeath()
    {
        boss.Empty();
    }
}
