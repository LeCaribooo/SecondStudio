using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_this_portal : MonoBehaviour
{

    static bool enable;
    public void enablle()
    {

        enable = true;

    }

    private void Update()
    {
        if (enable)
        {
            this.gameObject.SetActive(true);
        }
    }


}
