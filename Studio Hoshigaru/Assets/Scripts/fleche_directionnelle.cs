using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fleche_directionnelle : MonoBehaviour
{
    
    public Transform target;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = target.transform.position;
        targetPosition.z = transform.position.z;

        transform.LookAt(targetPosition);
    }
}
