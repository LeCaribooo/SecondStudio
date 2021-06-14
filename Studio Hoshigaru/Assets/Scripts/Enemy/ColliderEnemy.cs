using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEnemy : MonoBehaviour
{
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 10);
    }
}
