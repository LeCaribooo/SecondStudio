using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFlip : MonoBehaviour
{
    [SerializeField] Transform parent;
    void Update()
    {
        if(parent.localScale.x < 0)
        {
            if (this.transform.localScale.x > 0)
            {
                Vector3 Scaler = transform.localScale;
                Scaler.x *= -1;
                transform.localScale = Scaler;
            }
        }
        else
        {
            if (this.transform.localScale.x < 0)
            {
                Vector3 Scaler = transform.localScale;
                Scaler.x *= -1;
                transform.localScale = Scaler;
            }
        }
    }
}
